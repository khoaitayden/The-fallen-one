using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
public class ObstacleCreature1SpawnScript : MonoBehaviour
{
    [Header("Creature Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private float minHeight = -3.5f;
    [SerializeField] private float maxHeight = 3.5f;

    [Header("Movement Settings")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float minSpacing;
    [SerializeField] private float maxSpacing;
    public bool poolcheckspawned=false;
    [SerializeField] private Animator animator;
    public float Speed
    { 
        get { return movingSpeed; }
        set { movingSpeed = Mathf.Max(0, value); } // Ensure speed is not negative
    }
    private Queue<GameObject> CreaturePool;
    void Start()
    {
        //InitializePool();
    }
    void Update()
    {
        if (poolcheckspawned)MoveCreature();
    }

    public void InitializePool()
    {
        CreaturePool = new Queue<GameObject>();     
        for (int i=0;i<poolSize;i++)
        {
            SpawnCreature();
        }   
    }
    void SpawnCreature()
    {
        float randomY = Random.Range(minHeight, maxHeight);
        float randomX = Random.Range(minSpacing,maxSpacing);
        GameObject obj = Instantiate(obstaclePrefab);
        obj.SetActive(true);
        obj.transform.position = new Vector2(randomX, randomY);
        CreaturePool.Enqueue(obj);
    }
    void MoveCreature()
    {
    {
        foreach (GameObject obj in CreaturePool)
        {
            if (!obj.activeInHierarchy) continue;

            obj.transform.position += Vector3.left * movingSpeed * Time.deltaTime;

            if ((obj.transform.position.x <= -10) || (obj.transform.position.y<-6) ||(obj.transform.position.y>6))
            {
                ReuseCreature(obj);
            }
        }
    }
    }
    public void ReuseCreature(GameObject obstacle)
        {
            float randomY = Random.Range(minHeight, maxHeight);
            float randomX = Random.Range(minSpacing,maxSpacing);
            obstacle.transform.position = new Vector2(randomX, randomY);
            Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;  // Stop movement
            rb.angularVelocity = 0f; 
            rb.rotation = 0f;
            obstacle.SetActive(true);
        }
    public void IncreaseSpeedForObCreature1(float increment)
    {
        movingSpeed += increment;

        int neededObstacles = Mathf.FloorToInt(movingSpeed / 3f); // Add 1 extra every +2 speed
        int totalNeeded = poolSize + neededObstacles;

        // Spawn More Obstacles If Needed
        while (CreaturePool.Count < totalNeeded)
        {
            SpawnCreature();
        }
    }
}
