using UnityEngine;
public class ConfigObstacle1 : MonoBehaviour
{   
    [SerializeField] private Obstacle1 Obstacle1;
    [Header("Reuse Stats")]
    [SerializeField] private float maxSpacing;
    [SerializeField] private float minSpacing;   
    [SerializeField] private float gapTightness;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    private float lastX;
    int amountToPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("ReuseObstacle", 0f, 0.5f);
        amountToPool=CreateObstacle1.CreateObstacle1Instance.amountToPool;
        lastX =CreateObstacle1.CreateObstacle1Instance.pooledObjects[amountToPool-1].transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void ReuseObstacle()
    {
        for (int i=0;i<amountToPool;i++)
        {
            if (CreateObstacle1.CreateObstacle1Instance.pooledObjects[i].transform.position.x < -10)
            {
                float randomy= Random.Range(minHeight, maxHeight);
                if (i==0)
                {
                    lastX =CreateObstacle1.CreateObstacle1Instance.pooledObjects[amountToPool-1].transform.position.x;
                    
                } else lastX= CreateObstacle1.CreateObstacle1Instance.pooledObjects[i-1].transform.position.x;
                float spacing= Mathf.Clamp(maxSpacing-(Obstacle1.Obstacle1Speed * gapTightness), minSpacing, maxSpacing);
                CreateObstacle1.CreateObstacle1Instance.pooledObjects[i].transform.position= new Vector3(lastX+spacing, randomy, 0);
            }
        }
    }
}
