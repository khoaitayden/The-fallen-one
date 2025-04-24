using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
public class StateMangagerLv2 : MonoBehaviour
{
    public static StateMangagerLv2 Instance { get; private set; }
    [Header("State Setting")]
    [SerializeField] private float countdownTime;
    [SerializeField] private int HardScaleByTime;
    [SerializeField] private float HardIncreaseEachTime;
    [Header("Obstacle References")]
    [SerializeField] private GameObject obstacle3;
    [SerializeField] private Obstacle creature4;
    [Header("UI References")]
    [SerializeField] Text scoreText;
    [SerializeField] Text countdownText;
    private bool isCountingDown = true;
    private int lastHardnessTime = -1;
    void Update()
    {
        CountDown();
        IncreaseHardness();
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

        // Check if we've passed a new multiple of HardScaleByTime
        if (currentTime % HardScaleByTime == 0 && currentTime != lastHardnessTime && currentTime > 0)
        {
            lastHardnessTime = currentTime;
            DecreaseFallTime(HardIncreaseEachTime);
            UnityEngine.Debug.Log($"Fall time decreased to {TetrisBlock.fallTime} at {currentTime} seconds left.");
        }
    }
    void DecreaseFallTime(float amount)
    {
        TetrisBlock.fallTime -= amount;
    }
    
}
