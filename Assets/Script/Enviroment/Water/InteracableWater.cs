using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
//using UnityEditor.UIElements;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor.SearchService;
#endif

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(EdgeCollider2D))]
[RequireComponent(typeof(WaterTriggerHandler))]
public class InteracableWater : MonoBehaviour
{
    [Header("Spring")]
    [SerializeField] private float spriteConstant = 1.4f;
    [SerializeField] private float damping;
    [SerializeField] private float spread;
    [SerializeField, Range(1, 10)] private int wavePropagationIterations = 8;
    [SerializeField, Range(0f, 20f)] private float speedMult = 5.5f;
    [Header("Force")]
    public float ForceMultiplier = 0.2f;
    [Range(1f, 50f)] public float MaxForce = 5f;

    [Header("Collision")]
    [SerializeField, Range(1f, 10f)] private float playerCollisionRadiusMult = 4.15f;
    [Header("Mesh Generation")]
    [Range(2, 500)] public int NumofVertices = 100;
    public float Width = 10f;
    public float Height = 4f;
    public Material waterMaterial;
    
    [Header("Debug")]
    public Color gizmoColor = Color.white;

    private const int NUM_OF_Y_VERTICES = 2;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Vector3[] vertices;
    private int[] topVerticesIndex;
    private EdgeCollider2D edgeCollider;
    
    // Make nested class serializable to debug in inspector
    [System.Serializable]
    private class WaterPoint
    {
        public float velocity;
        public float acceleration;
        public float pos;
        public float targetHeight;
    }
    
    private List<WaterPoint> waterPoints = new List<WaterPoint>();
    
    // Exposed for debugging
    public int WaterPointCount => waterPoints.Count;
    
    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        GenerateMesh();
        CreateWaterPoints();
        
        // Ensure the collider is a trigger
        if (edgeCollider != null && !edgeCollider.isTrigger)
        {
            edgeCollider.isTrigger = true;
        }
    }

    private void Reset()
    {
        InitializeComponents();
        if (edgeCollider != null)
            edgeCollider.isTrigger = true;
    }

    private void OnValidate()
    {
        // Don't regenerate in play mode to avoid disrupting simulations
        if (!Application.isPlaying)
            GenerateMesh();
    }

    private void InitializeComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void ResetEdgeCollider()
    {
        if (edgeCollider == null || vertices == null || topVerticesIndex == null || topVerticesIndex.Length < 2)
        {
            return;
        }
        
        Vector2[] newPoints = new Vector2[2];

        Vector2 firstPoint = new Vector2(vertices[topVerticesIndex[0]].x, vertices[topVerticesIndex[0]].y);
        Vector2 secondPoint = new Vector2(vertices[topVerticesIndex[^1]].x, vertices[topVerticesIndex[^1]].y);

        newPoints[0] = firstPoint;
        newPoints[1] = secondPoint;

        edgeCollider.offset = Vector2.zero;
        edgeCollider.points = newPoints;
        edgeCollider.isTrigger = true;
    }

    public void GenerateMesh()
    {
        InitializeComponents();

        // Destroy old mesh to prevent memory leak
        if (meshFilter.sharedMesh != null && meshFilter.sharedMesh.name == "Water Mesh")
        {
            if (Application.isEditor)
                DestroyImmediate(meshFilter.sharedMesh);
            else
                Destroy(meshFilter.sharedMesh);
        }

        mesh = new Mesh
        {
            name = "Water Mesh"
        };

        vertices = new Vector3[NumofVertices * NUM_OF_Y_VERTICES];
        topVerticesIndex = new int[NumofVertices];

        for (int y = 0; y < NUM_OF_Y_VERTICES; y++)
        {
            for (int x = 0; x < NumofVertices; x++)
            {
                float xpos = (float)x / (NumofVertices - 1) * Width - Width / 2;
                float ypos = (float)y / (NUM_OF_Y_VERTICES - 1) * Height - Height / 2;
                int index = x + y * NumofVertices;
                vertices[index] = new Vector3(xpos, ypos, 0f);

                if (y == NUM_OF_Y_VERTICES - 1)
                {
                    topVerticesIndex[x] = index;
                }
            }
        }

        int[] triangles = new int[(NumofVertices - 1) * 6];
        int triIdx = 0;

        for (int x = 0; x < NumofVertices - 1; x++)
        {
            int bottomLeft = x;
            int bottomRight = x + 1;
            int topLeft = x + NumofVertices;
            int topRight = x + NumofVertices + 1;

            // Clockwise winding order
            triangles[triIdx++] = bottomLeft;
            triangles[triIdx++] = topLeft;
            triangles[triIdx++] = topRight;

            triangles[triIdx++] = bottomLeft;
            triangles[triIdx++] = topRight;
            triangles[triIdx++] = bottomRight;
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2((vertices[i].x + Width / 2) / Width, (vertices[i].y + Height / 2) / Height);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;

        if (waterMaterial == null)
        {
            var defaultMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if (defaultMaterial == null)
                defaultMaterial = new Material(Shader.Find("Standard"));
                
            defaultMaterial.color = new Color(0.3f, 0.7f, 0.9f, 0.8f);
            waterMaterial = defaultMaterial;
        }

        meshRenderer.material = waterMaterial;

        ResetEdgeCollider();
    }
    
    private void FixedUpdate()
    {
        if (waterPoints.Count == 0 || vertices == null || topVerticesIndex == null || mesh == null)
        {
            CreateWaterPoints();
            return;
        }

        //update all spring positions
        for (int i = 1; i < waterPoints.Count - 1; i++)
        {
            WaterPoint point = waterPoints[i];

            float x = point.pos - point.targetHeight;
            float acceleration = -spriteConstant * x - damping * point.velocity;
            point.pos += point.velocity * speedMult * Time.fixedDeltaTime;
            
            // Ensure the vertex index is valid
            if (i < topVerticesIndex.Length && topVerticesIndex[i] < vertices.Length)
                vertices[topVerticesIndex[i]].y = point.pos;
                
            point.velocity += acceleration * speedMult * Time.fixedDeltaTime;
        }

        //wave propagation
        for (int j = 0; j < wavePropagationIterations; j++)
        {
            for (int i = 1; i < waterPoints.Count - 1; i++)
            {
                float leftDelta = spread * (waterPoints[i].pos - waterPoints[i - 1].pos) * speedMult * Time.fixedDeltaTime;
                waterPoints[i - 1].velocity += leftDelta;

                float rightDelta = spread * (waterPoints[i].pos - waterPoints[i + 1].pos) * speedMult * Time.fixedDeltaTime;
                waterPoints[i + 1].velocity += rightDelta;
            }
        }

        //update the mesh
        mesh.vertices = vertices;
    }
    
    public void Splash(Collider2D collision, float force)
    {
        if (waterPoints.Count == 0)
        {
            Debug.LogWarning("No water points for splash effect");
            return;
        }
        
        float radius = collision.bounds.extents.x * playerCollisionRadiusMult;
        Vector2 center = collision.transform.position;

        bool appliedForce = false;
        
        for (int i = 0; i < waterPoints.Count; i++)
        {
            if (i >= topVerticesIndex.Length || topVerticesIndex[i] >= vertices.Length)
                continue;
                
            Vector2 vertexWorldPos = transform.TransformPoint(vertices[topVerticesIndex[i]]);

            if (IsPointInsideCircle(vertexWorldPos, center, radius))
            {
                waterPoints[i].velocity = force;
                appliedForce = true;
            }
        }
    }

    private bool IsPointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        float distanceSquared = (point - center).sqrMagnitude;
        return distanceSquared <= radius * radius;
    }
    
    private void CreateWaterPoints()
    {
        if (vertices == null || topVerticesIndex == null)
        {
            return;
        }
        
        waterPoints.Clear();

        for (int i = 0; i < topVerticesIndex.Length; i++)
        {
            if (topVerticesIndex[i] < vertices.Length)
            {
                waterPoints.Add(new WaterPoint
                {
                    pos = vertices[topVerticesIndex[i]].y,
                    targetHeight = vertices[topVerticesIndex[i]].y,
                    velocity = 0f,
                    acceleration = 0f
                });
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // Optionally visualize water points
        if (vertices != null && topVerticesIndex != null && Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < topVerticesIndex.Length; i++)
            {
                if (i < topVerticesIndex.Length && topVerticesIndex[i] < vertices.Length)
                    Gizmos.DrawSphere(transform.TransformPoint(vertices[topVerticesIndex[i]]), 0.05f);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InteracableWater))]
public class InteracableWaterEditor : Editor
{
    private InteracableWater water;

    private void OnEnable()
    {
        water = (InteracableWater)target;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        root.Add(new VisualElement { style = { height = 10 } });

        var generateButton = new Button(() => {
            water.GenerateMesh();
            EditorUtility.SetDirty(water);
        })
        {
            text = "Generate Mesh"
        };
        root.Add(generateButton);

        var colliderButton = new Button(() => {
            water.ResetEdgeCollider(); 
            EditorUtility.SetDirty(water);
        })
        {
            text = "Place Edge Collider"
        };
        root.Add(colliderButton);
        
        // Add debug info
        if (Application.isPlaying)
        {
            var debugLabel = new Label($"Water Points: {water.WaterPointCount}");
            root.Add(debugLabel);
        }

        return root;
    }

    private void ChangeDimensions(ref float width, ref float height, float calculatedWidthMax, float calculatedHeightMax)
    {
        width = Mathf.Max(0.1f, calculatedWidthMax);
        height = Mathf.Max(0.1f, calculatedHeightMax);
    }

    private void OnSceneGUI()
    {
        Handles.color = water.gizmoColor;

        Vector3 center = water.transform.position;
        Vector3 size = new Vector3(water.Width, water.Height, 0.1f);
        Handles.DrawWireCube(center, size);

        float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
        Vector3 snap = Vector3.one * 0.1f;

        Vector3[] corners = new Vector3[4]
        {
            center + new Vector3(-water.Width / 2, -water.Height / 2, 0),
            center + new Vector3(water.Width / 2, -water.Height / 2, 0),
            center + new Vector3(-water.Width / 2, water.Height / 2, 0),
            center + new Vector3(water.Width / 2, water.Height / 2, 0)
        };

        Vector3[] newCorners = new Vector3[4];

        for (int i = 0; i < 4; i++)
        {
            EditorGUI.BeginChangeCheck();
            newCorners[i] = Handles.FreeMoveHandle(corners[i], handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Vector3 opposite = corners[3 - i]; // 0 <-> 3, 1 <-> 2
                ChangeDimensions(ref water.Width, ref water.Height,
                    Mathf.Abs(newCorners[i].x - opposite.x),
                    Mathf.Abs(newCorners[i].y - opposite.y));

                water.transform.position = (newCorners[i] + opposite) / 2f;
                water.GenerateMesh();
                EditorUtility.SetDirty(water);
                break;
            }
        }
    }
}
#endif