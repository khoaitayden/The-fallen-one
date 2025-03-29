using UnityEngine;

public class HardController : MonoBehaviour
{

    private int lastpoint;
    //[Header("Obstacle1")]
    [SerializeField] Obstacle1 Obstacle1;
    [SerializeField] float Obstacle1AccelerateRate;
    void Start()
    {
        lastpoint=ScoreData.Instance.ScoreFromOb1;
    }
    void FixedUpdate()
    {
        if (lastpoint<ScoreData.Instance.ScoreFromOb1)
        {
            Obstacle1.Obstacle1Speed +=Obstacle1AccelerateRate;
            lastpoint=ScoreData.Instance.ScoreFromOb1;
        }
    }
}
