using UnityEngine;
using System.Collections.Generic;
public class CreateObstacle1 : MonoBehaviour
{
    public static CreateObstacle1 CreateObstacle1Instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        CreateObstacle1Instance =this;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            float randomy= Random.Range(-4.4f, 1.3f);
            Vector3 randomPosition = new Vector3(10+(i*8), randomy, 0);
            tmp = Instantiate(objectToPool,randomPosition, Quaternion.identity);
            pooledObjects.Add(tmp);
        }
    }
}
