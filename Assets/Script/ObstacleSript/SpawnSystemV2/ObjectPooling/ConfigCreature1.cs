using UnityEngine;

public class ConfigCreature1 : MonoBehaviour
{
    [SerializeField] private ObstacleCreature1 Creature1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReuseObstacle();
    }
    void ReuseObstacle()
    {

        for (int i=0;i<Creature1.Creature1Speed+1;i++)
        {
            if (CreateCreature1.CreateCreature1Instance.pooledObjects[i].activeInHierarchy == false)
            {
                CreateCreature1.CreateCreature1Instance.pooledObjects[i].SetActive(true);
            }
        }
        
    }
}
