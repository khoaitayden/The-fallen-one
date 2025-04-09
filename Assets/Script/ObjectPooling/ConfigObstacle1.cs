using UnityEngine;
using System.Collections.Generic;

public class ConfigObstacle1 : ConfigObstacle
{
    [SerializeField] private float maxSpacing;
    [SerializeField] private float minSpacing;   
    [SerializeField] private float gapTightness;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] CreateObstacle1 createObstacle1;
    [SerializeField] private Obstacle Obstacle1;


    public override void WrappReuse()
    {
        if (createObstacle1 == null || createObstacle1.pooledObjects == null || createObstacle1.pooledObjects.Count == 0) return;
        Reuse(createObstacle1.pooledObjects, createObstacle1.amountToPool);
    }
    public override Vector3 GenerateRandomPosition(int i)
    {
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
}
