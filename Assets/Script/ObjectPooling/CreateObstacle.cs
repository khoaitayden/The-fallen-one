using UnityEngine;
using System.Collections.Generic;
public abstract class CreateObstacle : MonoBehaviour
{
    //public static CreateObstacle CreateObstacleInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    
    public abstract Vector3 GenerateRandomPosition(int i);
    public abstract bool CheckCanSpawn();
    void Start()
    {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for(int i = 0; i < amountToPool; i++)
            {
                Vector3 randomPosition = GenerateRandomPosition(i);
                tmp = Instantiate(objectToPool,randomPosition, Quaternion.identity);
                if (!CheckCanSpawn()) tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
    }
}
