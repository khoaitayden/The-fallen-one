using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;
public class StateMangagerLv2 : MonoBehaviour
{
    public static StateMangagerLv2 Instance { get; private set; }
    public static event Action OnCountdownFinished;
    [Header("State Setting")]
    public float countdownTime;
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
                HardScaleByTime = 30;
                countdownTime = 200f;
                break;
            case 1:
                HardScaleByTime = 25;
                countdownTime = 225f;
                break;
            case 2:
                HardScaleByTime = 20;
                countdownTime = 250f;
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
                countdownText.text = Mathf.RoundToInt(countdownTime).ToString();
            }
            else
            {
                isCountingDown = false;
                OnCountdownFinished?.Invoke(); // <<<<< Fire the event
                UnityEngine.Debug.Log("Countdown finished! Event invoked.");
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
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
        }
    }
}
