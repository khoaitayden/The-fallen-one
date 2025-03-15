using UnityEngine;

public class DifficultyManagement : MonoBehaviour
{
    [Header("Obstacle Manager")]
    [SerializeField] private Obstacle1spawnscript obstacle1;
    [SerializeField] private ObstacleCreature1SpawnScript obstaclecreature1;

    private int lastscore=ScoreData.Instance.ScoreFromOb1;
    void Start()
    {
    }
    void Update()
    {

         if (ScoreData.Instance.ScoreFromOb1!=lastscore)
         {
             obstacle1.IncreaseSpeedForOb1(0.03f);
             lastscore=ScoreData.Instance.ScoreFromOb1;
             if (ScoreData.Instance.ScoreFromOb1>=50)
            {
                if (obstaclecreature1.poolcheckspawned==false)
                {
                    obstaclecreature1.InitializePool();
                    obstaclecreature1.poolcheckspawned=true;
                }
                obstaclecreature1.IncreaseSpeedForObCreature1(0.05f);
            }
         }
         
    }
}
