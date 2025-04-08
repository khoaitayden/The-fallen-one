using UnityEngine;

public class HardController : MonoBehaviour
{

    private int lastpoint;
//    [SerializeField] Obstacle1 Obstacle1;
//    [SerializeField] ObstacleCreature1 Creature1;
    [SerializeField] float Obstacle1AccelerateRate;
    [SerializeField] float ObstacleCreature1AccelerateRate;
    public float AfterAcceleratedObstacle1;
    public float AfterAccleratedObstacleCreature1;
    void Start()
    {
        lastpoint=ScoreManager.Instance.Score;
    }
    // void FixedUpdate()
    // {
    //     if (lastpoint<ScoreManager.Instance.Score)
    //     {
    //         AfterAccleratedObstacleCreature1=Creature1.Creature1Speed+ObstacleCreature1AccelerateRate;
    //         AfterAcceleratedObstacle1= Obstacle1.Obstacle1Speed +Obstacle1AccelerateRate;
    //         lastpoint=ScoreManager.Instance.Score;
    //     }
    // }
}
