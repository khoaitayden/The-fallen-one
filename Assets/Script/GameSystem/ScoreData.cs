using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public static ScoreData Instance { get; private set; }
    private int scoreFromOb1;
    void Start()
    {
        scoreFromOb1=0;
    }

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

    public int ScoreFromOb1
    {
        get { return scoreFromOb1; }
        set { scoreFromOb1 = value; }
    }

    public void AddScoreToOb1(int amount)
    {
        scoreFromOb1 += amount;
    }
}