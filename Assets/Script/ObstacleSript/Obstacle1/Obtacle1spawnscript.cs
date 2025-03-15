using UnityEngine;
using System.Collections.Generic;

public class Obstacle1spawnscript : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize; // Base obstacle count
    [SerializeField] private float minHeight = -3.8f;
    [SerializeField] private float maxHeight = 1.8f;
    [SerializeField] private float gapTightness ;
    //A higher gapTightness value makes the gap shrink faster when speed increases.
    // A lower gapTightness value slows down the gap reduction, keeping obstacles more spread out.
    //A value of 0 means the gap never changes, while a higher value (e.g., 0.6f or 0.8f) makes obstacles appear closer together as speed increases.

    [Header("Movement Settings")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float minSpacing = 2f;
    [SerializeField] private float maxSpacing = 5f;

    private Queue<GameObject> obstaclePool;
    private GameObject lastSpawnedObstacle = null; // Track last active obstacle

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        MoveObstacles();
    }

    // Initialize Pool and Spawn Base Obstacles
    void InitializePool()
    {
        obstaclePool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
           SpawnObstacle();
        }
    }

    // Move Active Obstacles & Reuse When Off-Screen
    void MoveObstacles()
    {
        foreach (GameObject obj in obstaclePool)
        {
            if (!obj.activeInHierarchy) continue;

            obj.transform.position += Vector3.left * movingSpeed * Time.deltaTime;

            if (obj.transform.position.x <= -10)
            {
                ReuseObstacle(obj);
            }
        }
    }

    // Spawn Initial Obstacles
    void SpawnObstacle()
    {

        float randomY = Random.Range(minHeight, maxHeight);

        // Calculate Fixed Spacing Based on Speed
        float dynamicSpacing = Mathf.Clamp(maxSpacing - (movingSpeed * gapTightness), minSpacing, maxSpacing);

        // Ensure Consistent Spawn Position
        float lastX = 10f; // Default first spawn position

        if (lastSpawnedObstacle != null && lastSpawnedObstacle.activeInHierarchy)
        {
            lastX = lastSpawnedObstacle.transform.position.x;
        }

        float newX = lastX + dynamicSpacing;

        GameObject obj = Instantiate(obstaclePrefab);
        obj.SetActive(true);
        obj.transform.position = new Vector2(newX, randomY);
  
        lastSpawnedObstacle = obj; // Update last spawned obstacle
        obstaclePool.Enqueue(obj);
    }

    // Reuse Obstacles Dynamically
    void ReuseObstacle(GameObject obstacle)
    {
        float randomY = Random.Range(minHeight, maxHeight);

        // Calculate Dynamic Spacing Based on Speed
        float dynamicSpacing = Mathf.Clamp(maxSpacing - (movingSpeed * gapTightness), minSpacing, maxSpacing);

        // Determine Last Obstacle Position
        float lastX = 10f; // Default spawn position

        if (lastSpawnedObstacle != null && lastSpawnedObstacle.activeInHierarchy)
        {
            lastX = lastSpawnedObstacle.transform.position.x;
        }

        float newX = lastX + dynamicSpacing;
        obstacle.transform.position = new Vector2(newX, randomY);
        obstacle.SetActive(true);

        lastSpawnedObstacle = obstacle; // Update last spawned obstacle
    }

    // Increase Speed & Add More Obstacles Dynamically
    public void IncreaseSpeedForOb1(float increment)
    {
        movingSpeed += increment;

        int neededObstacles = Mathf.FloorToInt(movingSpeed / 6f); // Add 1 extra every +6 speed
        int totalNeeded = poolSize + neededObstacles;

        // Spawn More Obstacles If Needed
        while (obstaclePool.Count < totalNeeded)
        {
            SpawnObstacle();
        }
    }
}
