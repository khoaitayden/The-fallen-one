using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ConfigObstacle1 : ConfigObstacle
{
    [SerializeField] private float maxSpacing;
    [SerializeField] private float minSpacing;   
    [SerializeField] private float gapTightness;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] CreateObstacle1 createObstacle1;
    [SerializeField] private Obstacle Obstacle1;
    private int reuseAmount=0;

    public override void WrappReuse()
    {
        if (createObstacle1 == null || createObstacle1.pooledObjects == null || createObstacle1.pooledObjects.Count == 0) return;
        if (!(StateManager.Instance.stage3ScoreReqirement>reuseAmount+createObstacle1.amountToPool)) 
        {
            return;
        }
        Reuse(createObstacle1.pooledObjects, createObstacle1.amountToPool);
    }
    public override Vector3 GenerateRandomPosition(int i)
    {
        reuseAmount++;
        float lastX;
        if (i==0)
        {
            lastX=createObstacle1.pooledObjects[createObstacle1.amountToPool-1].transform.position.x;
        } else lastX =createObstacle1.pooledObjects[i-1].transform.position.x;
            float randomY = Random.Range(minHeight, maxHeight);
            float spacing= Mathf.Clamp(maxSpacing-(Obstacle1.movespeed*Obstacle.SpeedMultiplier* gapTightness), minSpacing, maxSpacing);
            Vector3 randomPosition = new Vector3(lastX+spacing, randomY, 0);
            return randomPosition;
    }
    private void OnEnable()
    {
        PlayerBehavior.OnPlayerDied += ResetReuseAmount;
    }

    private void OnDisable()
    {
        PlayerBehavior.OnPlayerDied -= ResetReuseAmount;
    }

    private void ResetReuseAmount()
    {
        reuseAmount = 0;
    }
    public void Destroyall()
    {
        for (int i = 0; i < createObstacle1.pooledObjects.Count; i++)
        {
            Destroy(createObstacle1.pooledObjects[i]);
        }
        createObstacle1.pooledObjects.Clear();
    }
}
