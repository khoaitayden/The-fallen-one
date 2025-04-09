using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
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
        scoreText.text=passedObstacle.ToString();
        Obstacle.SpeedMultiplier += 0.005f;
        if ((passedObstacle>=50)&&(createCreature1.pooledObjects[1].activeSelf==false))
        {
            for (int i = 0; i < createCreature1.amountToPool; i++)
            {
                createCreature1.pooledObjects[i].SetActive(true);
            }
        }        
        if (passedObstacle>=150)
        {
            for (int i = 0; i < createObstacle1.amountToPool; i++)
            {
                Destroy(createObstacle1.pooledObjects[i]);
            }
        }

    }
}