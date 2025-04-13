using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }
    [Header("State Setting")]
    public int stage2ScoreReqirement;
    public int stage3ScoreReqirement;
    [Header("State References")]
    [SerializeField] private GameObject stage3;
    [Header("Obstacle References")]
    [SerializeField] private Obstacle obstacle1;
    [SerializeField] private Obstacle creature1;
    [Header("Object Pooling References")]
    [SerializeField] CreateCreature1 createCreature1;
    [SerializeField] CreateObstacle1 createObstacle1;
    [Header("UI References")]
    [SerializeField] Text scoreText;

    private int passedObstacle;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        passedObstacle=0;

    }
    void FixedUpdate()
    {
        scoreText.text=passedObstacle.ToString();
    }
    public int Score
{
    get { return passedObstacle; }
    set
    {
        passedObstacle = value;
        if (passedObstacle < 0)
        {
            PlayerBehavior.TriggerPlayerDied();
        }
    }
}
    public void ResetScore()
    {
        passedObstacle = 0;
    }

    public void IncreaseHardAfterPassedPipe(int amount)
    {
        passedObstacle+= amount;
        Obstacle.SpeedMultiplier += 0.005f;
        if ((passedObstacle>=stage2ScoreReqirement)&&(createCreature1.pooledObjects[1].activeSelf==false))
        {
            Stage2();
        }        
        if (passedObstacle>=stage3ScoreReqirement)
        {
            Stage3();
        }
    }

    void Stage2()
    {
        for (int i = 0; i < createCreature1.amountToPool; i++)
            {
                createCreature1.pooledObjects[i].SetActive(true);
            }
    }
    void Stage3()
    {
        for (int i = 0; i < createObstacle1.amountToPool; i++)
            {
                stage3.SetActive(true);
            }
    }
}