using UnityEngine;
using System.Collections;

public class Bossstage : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject creature2;
    [SerializeField] private GameObject creature3;
    [SerializeField] private GameObject ceiling;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Creature2 creature2Script;
    [SerializeField] private SpriteRenderer background2;
    [SerializeField] private SpriteRenderer background3;
    [SerializeField] private SpriteRenderer background4;
    [SerializeField] private Animator creature2Animator;
    
    private bool hasInitialized = false;
    private bool sequenceStarted = false;

    private float alpha = 0f;
    private float audioVolume = 0f;
    
    private Vector3 ceilingMoveStep;
    private Vector3 floorMoveStep;
    private Vector3 creature2MoveStep;
    private Vector3 playerMoveStep;
    private Color reusableColor;
    private Transform ceilingTransform;
    private Transform floorTransform;
    private Transform creature2Transform;
    private Transform playerTransform;
    private WaitForSeconds waitInterval;

    private void Awake()
    {
        ceilingMoveStep = new Vector3(0.5f, 0f, 0f);
        floorMoveStep = new Vector3(0.5f, 0f, 0f);
        creature2MoveStep = new Vector3(0.3f, 0f, 0f);
        playerMoveStep = new Vector3(0.1f, 0f, 0f);
        
        if (ceiling) ceilingTransform = ceiling.transform;
        if (floor) floorTransform = floor.transform;
        if (creature2) creature2Transform = creature2.transform;
        if (player) playerTransform = player.transform;
        
        reusableColor = new Color();
        waitInterval = new WaitForSeconds(0.1f);
    }

    void Start()
    {
        if (Level1Loader.PreLoaded)
        {
            PreInitialize();
        }
    }
    
    private void PreInitialize()
    {
        if (!hasInitialized && creature2Script != null)
        {
            creature2Script.OnSpeedChanged += OnCreatureSpeedChanged;
            hasInitialized = true;
        }
        
        if (creature3 != null) creature3.SetActive(false);
        if (ball != null) ball.SetActive(false);
        
        if (audioSource != null)
        {
            audioSource.volume = 0f;
        }
        

        ResetObjectPositions();
    }
    
    private void ResetObjectPositions()
    {

        if (ceiling && ceilingTransform)
        {
            Vector3 pos = ceilingTransform.position;
            ceilingTransform.position = new Vector3(20f, pos.y, pos.z);
        }
        
        if (floor && floorTransform)
        {
            Vector3 pos = floorTransform.position;
            floorTransform.position = new Vector3(20f, pos.y, pos.z);
        }
        
        if (creature2 && creature2Transform)
        {
            Vector3 pos = creature2Transform.position;
            creature2Transform.position = new Vector3(20f, pos.y, pos.z);
        }
    }
    
    void OnEnable()
    {
        if (Level1Loader.PreLoaded && !sequenceStarted)
        {
            if (StateManager.Instance && StateManager.Instance.stage3Activated)
            {
                StartBossSequence();
            }
        }
    }
    
    public void StartBossSequence()
    {
        if (sequenceStarted) return;
        
        if (!hasInitialized)
        {
            PreInitialize();
        }
        
        sequenceStarted = true;
        
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        ResetObjectPositions();
        
        StartCoroutine(CeilingAndFloorComeOut());
    }

    private IEnumerator CeilingAndFloorComeOut()
    {
        Vector3 ceilingPos, floorPos;
        
        while (ceilingTransform.position.x >= 0.5f || floorTransform.position.x >= 0.5f)
        {

            ceilingPos = ceilingTransform.position;
            floorPos = floorTransform.position;
            
            ceilingTransform.position = ceilingPos - ceilingMoveStep;
            floorTransform.position = floorPos - floorMoveStep;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            audioVolume = Mathf.Clamp01(audioVolume + 0.02f);
            
            reusableColor = background2.color;
            reusableColor.a = alpha;
            background2.color = reusableColor;
            
            // Update audio volume
            if (audioSource != null)
                audioSource.volume = audioVolume;
            
            yield return waitInterval;
        }
        
        // Check hardmode setting
        if(StartMenu.hardmode == 0)
            DestroyCreature1();
            
        StartCoroutine(BossComeOut());
    }

    private IEnumerator BossComeOut()
    {
        Vector3 creaturePos;
        
        while (creature2Transform.position.x > 7f)
        {
            creaturePos = creature2Transform.position;
            creature2Transform.position = creaturePos - creature2MoveStep;
            yield return waitInterval;
        }
        
        if (ball != null)
            ball.SetActive(true);
    }

    void OnCreatureSpeedChanged(int newSpeed)
    {
        if (newSpeed == ChooseHard(StartMenu.hardmode))
        {
            if (ball != null)
                Destroy(ball);
            
            // Optimize by using the cached transform
            Vector3 pos = creature2Transform.position;
            creature2Transform.position = new Vector3(pos.x, 0f, pos.z);
            
            alpha = 0f;
            StartCoroutine(TheEndCutScene());
        }
    }
    
    private int ChooseHard(int hardmode)
    {
        switch (hardmode)
        {
            case 0: return 5;
            case 1: return 6;
            case 2: return 7;
            default: return 0;
        }
    }

    private IEnumerator TheEndCutScene()
    {
        DestroyCreature1();
        
        Vector3 ceilingPos, floorPos;
        
        while (ceilingTransform.position.x > -20f || floorTransform.position.x > -20f)
        {
            ceilingPos = ceilingTransform.position;
            floorPos = floorTransform.position;
            
            ceilingTransform.position = ceilingPos - ceilingMoveStep;
            floorTransform.position = floorPos - floorMoveStep;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            
            // Update background3 color using reusable color
            reusableColor = background3.color;
            reusableColor.a = alpha;
            background3.color = reusableColor;
            
            yield return waitInterval;
        }
        
        if (ceiling != null)
            Destroy(ceiling);
            
        if (floor != null)
            Destroy(floor);

        creature2Animator.SetTrigger("death");
        creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;

        alpha = 0f;
        StartCoroutine(PlayerMoveMiddle());
    }

    private IEnumerator PlayerMoveMiddle()
    {
        Vector3 playerPos;
        
        while (playerTransform.position.x <= 0f)
        {
            playerPos = playerTransform.position;
            playerTransform.position = playerPos + playerMoveStep;
            
            // Update background4 color using reusable color
            reusableColor = background4.color;
            reusableColor.a = alpha;
            background4.color = reusableColor;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            
            yield return waitInterval;
        }
        
        if (creature3 != null)
            creature3.SetActive(true);
            
        StartCoroutine(CheckGoToLv2());
    }

    private IEnumerator CheckGoToLv2()
    {
        while (creature3 != null)
        {
            yield return waitInterval;
        }
        StateManager.Instance.goToLv2();
    }
    
    void DestroyCreature1()
    {
        ConfigCreature1.canreuse = false;
    }
    
    void OnDisable()
    {
        // Don't stop coroutines if we're in prewarm mode
        if (sequenceStarted)
        {
            StopAllCoroutines();
        }
    }
    
    void OnDestroy()
    {
        // Clean up event handler
        if (creature2Script != null)
        {
            creature2Script.OnSpeedChanged -= OnCreatureSpeedChanged;
        }
    }
}