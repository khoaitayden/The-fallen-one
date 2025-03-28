using UnityEngine;

public class DifficultyManagement : MonoBehaviour
{
    [Header("Obstacle Manager")]
    [SerializeField] private Obstacle1spawnscript obstacle1;
    [SerializeField] private ObstacleCreature1SpawnScript obstaclecreature1;
    [SerializeField] private ObstacleEliteCreature1Script obstacleelitecreature1;

    private int lastscore=ScoreData.Instance.ScoreFromOb1;
    void Start()
    {
    }
    void FixedUpdate()
    {

         if (ScoreData.Instance.ScoreFromOb1!=lastscore)
         {
            obstacle1.IncreaseSpeedForOb1(0.03f);
            lastscore=ScoreData.Instance.ScoreFromOb1;
            if (ScoreData.Instance.ScoreFromOb1>=0)
             //the amount of score needed to spawn the creature
            {
                if (obstaclecreature1.poolcheckspawned==false)
                {
                    obstaclecreature1.poolcheckspawned=true;
                }
                obstaclecreature1.IncreaseSpeedForObCreature1(0.05f);
            }
            if (ScoreData.Instance.ScoreFromOb1>=2)
             //the amount of score needed to spawn the creature
            {
                if (obstacleelitecreature1.poolcheckspawned==false)
                {
                    obstacleelitecreature1.poolcheckspawned=true;
                }
                obstacleelitecreature1.IncreaseSpeedForObEliteCreature1(0.05f);
            }
         }
         
    }
}
