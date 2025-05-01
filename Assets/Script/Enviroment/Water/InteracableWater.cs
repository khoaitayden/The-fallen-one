using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(EdgeCollider2D))]
[RequireComponent(typeof(WaterTriggerHandler))]
public class InteracableWater : MonoBehaviour
{
    [Header("Mesh Generation")]
    [Range(2, 500)] public int NumofVertices = 100;
    public float Width = 10f;
    public float Height = 4f;
    public Material waterMaterial;

    [Header("Gizmo")]
    public Color gizmoColor = Color.white;

    private const int NUM_OF_Y_VERTICES = 2;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Vector3[] vertices;
    private int[] topVerticesIndex;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        GenerateMesh();
    }

    private void Reset()
    {
        InitializeComponents();
        edgeCollider.isTrigger = true;
    }

    private void OnValidate()
    {
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
        Vector2[] newPoints = new Vector2[2];

        Vector2 firstPoint = new Vector2(vertices[topVerticesIndex[0]].x, vertices[topVerticesIndex[0]].y);
        Vector2 secondPoint = new Vector2(vertices[topVerticesIndex[^1]].x, vertices[topVerticesIndex[^1]].y);

        newPoints[0] = firstPoint;
        newPoints[1] = secondPoint;

        edgeCollider.offset = Vector2.zero;
        edgeCollider.points = newPoints;
    }

    public void GenerateMesh()
    {
        InitializeComponents();

        // Destroy old mesh to prevent memory leak
        if (meshFilter.sharedMesh != null)
        {
            DestroyImmediate(meshFilter.sharedMesh);
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

        // Optional: assign debug material if none is set
        if (waterMaterial == null)
        {
            var defaultMaterial = new Material(Shader.Find("Unlit/Color"));
            defaultMaterial.color = Color.cyan;
            waterMaterial = defaultMaterial;
        }

        meshRenderer.material = waterMaterial;

        ResetEdgeCollider();
    }
}
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

        var generateButton = new Button(() => water.GenerateMesh())
        {
            text = "Generate Mesh"
        };
        root.Add(generateButton);

        var colliderButton = new Button(() => water.ResetEdgeCollider())
        {
            text = "Place Edge Collider"
        };
        root.Add(colliderButton);

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
                break;
            }
        }
    }
}