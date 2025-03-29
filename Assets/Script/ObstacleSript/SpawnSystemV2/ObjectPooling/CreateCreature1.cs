using UnityEngine;
using System.Collections.Generic;
public class CreateCreature1 : MonoBehaviour
{
    public static CreateCreature1 CreateCreature1Instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        CreateCreature1Instance =this;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            float randomy= Random.Range(-3.8f, 3.8f);
            Vector3 randomPosition = new Vector3(10+(i*8), randomy, 0);
            tmp = Instantiate(objectToPool,randomPosition, Quaternion.identity);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    


}
