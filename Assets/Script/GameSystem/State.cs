using UnityEngine;
using UnityEngine.Rendering;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private Obstacle obstacle1;
    [SerializeField] private Obstacle creature1;
    [SerializeField] CreateCreature1 createCreature1;
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
    public int Score
    {
        get {return passedObstacle;}
    }
    public void ResetScore()
    {
        passedObstacle = 0;
    }

    public void IncreaseHardAfterPassedPipe(int amount)
    {
        passedObstacle+= amount;
        Obstacle.SpeedMultiplier += 0.005f;
        if ((passedObstacle>=10)&&(createCreature1.pooledObjects[1].activeSelf==false))
        {
            for (int i = 0; i < createCreature1.amountToPool; i++)
            {
                createCreature1.pooledObjects[i].SetActive(true);
            }
        }
    }
}