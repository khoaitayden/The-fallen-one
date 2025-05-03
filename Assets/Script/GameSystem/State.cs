using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }
    
    [Header("State Setting")]
    public int stage2ScoreReqirement;
    public int stage3ScoreReqirement;
    
    [Header("State References")]
    [SerializeField] private GameObject stage3;
    [SerializeField] private AudioSource audioSourceStage2;
    
    [Header("Obstacle References")]
    [SerializeField] private Obstacle obstacle1;
    [SerializeField] private Obstacle creature1;
    
    [Header("Object Pooling References")]
    [SerializeField] CreateCreature1 createCreature1;
    [SerializeField] CreateObstacle1 createObstacle1;
    
    [Header("UI References")]
    [SerializeField] Text scoreText;
    [SerializeField] SceneTransition scenetransition;

    private int score;
    private bool stage2Activated = false;
    private bool stage3Activated = false;

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
        
        // Pre-initialize stage3 but keep it inactive
        if (stage3 != null)
        {
            stage3.SetActive(false);
        }
    }
    
    void Start()
    {
        score = 0;
        ChoosedHard(StartMenu.hardmode);
    }
    
    void FixedUpdate()
    {
        if (scoreText.text != score.ToString())
        {
            scoreText.text = score.ToString();
        }
    }
    
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            if (score < 0)
            {
                PlayerBehavior.TriggerPlayerDied();
            }
        }
    }
    
    public void ResetScore()
    {
        score = 0;
    }
    
    private void ChoosedHard(int hardmode)
    {
        switch (hardmode)
        {
            case 0:
                Debug.Log("Hardmode easy chosen: " + StartMenu.hardmode);
                stage2ScoreReqirement = 5;
                stage3ScoreReqirement = 10;
                break;
            case 1:
                Debug.Log("Hardmode normal chosen: " + StartMenu.hardmode);
                stage2ScoreReqirement = 50;
                stage3ScoreReqirement = 100;
                break;
            case 2:
                Debug.Log("Hardmode hard chosen: " + StartMenu.hardmode);
                stage2ScoreReqirement = 100;
                stage3ScoreReqirement = 150;
                break;
        }
    }
    
    public void IncreaseHardAfterPassedPipe(int amount)
    {
        score += amount;
        Obstacle.SpeedMultiplier += 0.005f;
        
        // Only check and activate stage2 once
        if (!stage2Activated && score >= stage2ScoreReqirement)
        {
            ActivateStage2();
        }
        
        // Only check and activate stage3 once
        if (!stage3Activated && score >= stage3ScoreReqirement)
        {
            StartCoroutine(ActivateStage3());
        }
    }

    void ActivateStage2()
    {
        stage2Activated = true;
        
        // Start a coroutine to activate objects gradually to prevent lag spike
        StartCoroutine(GraduallyActivateCreatures());
    }
    
    private IEnumerator GraduallyActivateCreatures()
    {
        for (int i = 0; i < createCreature1.amountToPool; i++)
        {
            createCreature1.pooledObjects[i].SetActive(true);
            // Wait a frame between each activation to distribute the processing load
            yield return null;
        }
    }
    
    private IEnumerator ActivateStage3()
    {
        stage3Activated = true;
        StartCoroutine(SoundFadeOut(audioSourceStage2, 1f));
        if (stage3 != null)
        {
            yield return null;
            stage3.SetActive(true);
        }
    }
    
    public void goToLv2()
    {
        scenetransition.sceneTransition("level2");
        PlayerBehavior.isDead = false;
    }
    
    IEnumerator SoundFadeOut(AudioSource audioSource, float duration)
    {
        if (audioSource == null)
            yield break;
            
        float startVolume = audioSource.volume;
        float startTime = Time.time;
        
        while (Time.time < startTime + duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, (Time.time - startTime) / duration);
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}