using UnityEngine;
using System.Collections.Generic;

public class Obstacle1spawnscript : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize = 10; 
    [SerializeField] private float minHeight = -3.8f;
    [SerializeField] private float maxHeight = 1.8f;
    [SerializeField] private float gapTightness = 0.5f;

    [Header("Movement Settings")]
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float minSpacing = 2f;
    [SerializeField] private float maxSpacing = 5f;

    private GameObject[] obstaclePool; // Faster access than Queue/List
    private int poolIndex = 0;
    private float lastXPosition = 10f; // Track the last obstacle position

    void Start()
    {
        InitializePool();
    }

    void FixedUpdate()
    {
        MoveObstacles();
    }

    // Efficiently Initialize Object Pool
    void InitializePool()
    {
        obstaclePool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab,new Vector3(12,0,0),Quaternion.identity);
            obj.SetActive(false);
            obstaclePool[i] = obj;
        }
    }

    // Move Active Obstacles & Reuse When Off-Screen
    void MoveObstacles()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = obstaclePool[i];
            obj.SetActive(true);
            if (!obj.activeInHierarchy) continue;

            obj.transform.position += Vector3.left * movingSpeed * Time.deltaTime;

            if (obj.transform.position.x <= -10)
            {
                ReuseObstacle(obj);
            }
        }
    }
    // Reuse Obstacles When They Leave the Screen
    void ReuseObstacle(GameObject obstacle)
    {
        float randomY = Random.Range(minHeight, maxHeight);
        float spacing = Mathf.Clamp(maxSpacing - (movingSpeed * gapTightness), minSpacing, maxSpacing);
        lastXPosition += spacing;

        obstacle.transform.position = new Vector2(lastXPosition, randomY);
        obstacle.SetActive(true);
    }

    // Increase Speed & Dynamically Adjust Pool
    public void IncreaseSpeedForOb1(float increment)
    {
        movingSpeed += increment;

        int newPoolSize = Mathf.FloorToInt(movingSpeed / 6f) + poolSize;

        if (newPoolSize > obstaclePool.Length)
        {
            ExpandPool(newPoolSize);
        }
    }

    // Expand Object Pool If Needed
    void ExpandPool(int newSize)
    {
        GameObject[] newPool = new GameObject[newSize];

        for (int i = 0; i < obstaclePool.Length; i++)
        {
            newPool[i] = obstaclePool[i]; // Copy existing obstacles
        }

        for (int i = obstaclePool.Length; i < newSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab,new Vector3(12,0,0),Quaternion.identity);
            //obj.SetActive(true);
            newPool[i] = obj;
        }

        obstaclePool = newPool;
        poolSize = newSize;
    }
}