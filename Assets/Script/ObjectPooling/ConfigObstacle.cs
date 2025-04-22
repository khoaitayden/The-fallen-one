using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigObstacle : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("WrappReuse", 0f, 1f);
    }
    public abstract Vector3 GenerateRandomPosition(int i);
    public abstract void WrappReuse();
    public void Reuse(List<GameObject> pooledObjects, int amountToPool)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i] == null || pooledObjects == null || pooledObjects.Count == 0) return;
            if (pooledObjects[i].transform.position.x < -10f) 
            {
                Vector3 randomPosition = GenerateRandomPosition(i);
                pooledObjects[i].transform.position = randomPosition;
            }
        }
    }
    
}