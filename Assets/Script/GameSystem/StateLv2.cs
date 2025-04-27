using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
public class StateMangagerLv2 : MonoBehaviour
{
    public static StateMangagerLv2 Instance { get; private set; }
    [Header("State Setting")]
    [SerializeField] private float countdownTime;
    public int HardScaleByTime;
    [SerializeField] private float HardIncreaseEachTime;
    [Header("Obstacle References")]
    [SerializeField] private GameObject obstacle3;
    [SerializeField] private Obstacle creature4;
    [Header("UI References")]
    [SerializeField] Text scoreText;
    [SerializeField] Text countdownText;
    private bool isCountingDown = true;
    private int lastHardnessTime = -1;
    private int score;
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
        score=StateManager.Instance.Score;
        UnityEngine.Debug.Log(score);
        ChoosedHard(StartMenu.hardmode);
    }
    void FixedUpdate()
    {
        if(PlayerBehavior.isDead)
        {
            return;
        }
        CountDown();
        IncreaseHardness();
        scoreText.text = score.ToString();
    }
    private void ChoosedHard(int hardmode)
    {
        switch (hardmode)
        {
            case 0:
                HardScaleByTime = 25;
                break;
            case 1:
                HardScaleByTime = 20;
                break;
            case 2:
                HardScaleByTime = 15;
                break;
            default:
                break;
        }
    }
    void CountDown()
    {
        if (isCountingDown)
    {
        countdownTime -= Time.deltaTime;
        if (countdownTime > 0)
        {
            countdownText.text =Mathf.RoundToInt(countdownTime).ToString();
        }
        else
        {
            isCountingDown = false;
        }
    }
    }

    void IncreaseHardness()
    {
        int currentTime = Mathf.FloorToInt(countdownTime);
        if (currentTime % HardScaleByTime == 0 && currentTime != lastHardnessTime && currentTime > 0)
        {
            lastHardnessTime = currentTime;
            DecreaseFallTime(HardIncreaseEachTime);
            //UnityEngine.Debug.Log($"Fall time decreased to {TetrisBlock.fallTime} at {currentTime} seconds left.");
        }
    }
    void DecreaseFallTime(float amount)
    {
        TetrisBlock.fallTime -= amount;
    }

    public void ResetScoreLv2()
    {
        score = StateManager.Instance.Score;
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
    }
}
