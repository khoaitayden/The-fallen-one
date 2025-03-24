using UnityEngine;
using System.Collections.Generic;

public class ObstacleCreature1SpawnScript : MonoBehaviour
{
    [Header("Creature Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private float minHeight = -3.5f;
    [SerializeField] private float maxHeight = 3.5f;
    [SerializeField] PlayerBehavior playerBehavior;

    [Header("Movement Settings")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float minSpacing;
    [SerializeField] private float maxSpacing;
    public bool poolcheckspawned = false;
    [SerializeField] private Animator animator;
    
    public List<GameObject> CreaturePool;
//    private List<Rigidbody2D> rigidbodies;
    
    public float Speed
    {
        get { return movingSpeed; }
        set { movingSpeed = Mathf.Max(0, value); } // Ensure speed is not negative
    }
    
    void Start()
    {
        InitializePool();
    }

    void FixedUpdate()
    {
        if (poolcheckspawned) MoveCreature();
    }

    private void InitializePool()
    {
        CreaturePool = new List<GameObject>(poolSize);
//        rigidbodies = new List<Rigidbody2D>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab,new Vector3(12,0,0),Quaternion.identity);
            obj.SetActive(false);
            CreaturePool.Add(obj);
//            rigidbodies.Add(obj.GetComponent<Rigidbody2D>());
        }
    }

    private void MoveCreature()
    {
        for (int i = 0; i < CreaturePool.Count; i++)
        {
            GameObject obj = CreaturePool[i];
            obj.SetActive(true);
            if (!obj.activeInHierarchy) continue;
            obj.transform.position += Vector3.left * movingSpeed * Time.deltaTime;
            if (obj.transform.position.x <= -10 || obj.transform.position.y < -6 || obj.transform.position.y > 6)
            {
                ReuseCreature(i);
            }
        }
    }

    public void ReuseCreature(int index)
    {
        GameObject obstacle = CreaturePool[index];
//        Rigidbody2D rb = rigidbodies[index];

        float randomY = Random.Range(minHeight, maxHeight);
        float randomX = Random.Range(minSpacing, maxSpacing);

        obstacle.transform.position = new Vector2(randomX, randomY);
        // rb.linearVelocity = Vector2.zero;  // Stop movement
        // rb.angularVelocity = 0f;
        // rb.rotation = 0f;

        obstacle.SetActive(true);
    }

    public void IncreaseSpeedForObCreature1(float increment)
    {
        movingSpeed += increment;
        int neededObstacles = Mathf.FloorToInt(movingSpeed / 5f);
        int totalNeeded = poolSize + neededObstacles;

        while (CreaturePool.Count < totalNeeded)
        {
            GameObject obj = Instantiate(obstaclePrefab,new Vector3(12,0,0),Quaternion.identity);;
            obj.SetActive(false);
            CreaturePool.Add(obj);
//            rigidbodies.Add(obj.GetComponent<Rigidbody2D>());
        }
    }
}