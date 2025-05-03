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
    [SerializeField] private Animator creauture2Animator;

    private float alpha = 0f;
    private float audioVolume = 0f;
    
    private Vector3 ceilingMoveStep;
    private Vector3 floorMoveStep;
    private Vector3 creature2MoveStep;
    private Vector3 playerMoveStep;

    private void Awake()
    {
        ceilingMoveStep = new Vector3(0.5f, 0f, 0f);
        floorMoveStep = new Vector3(0.5f, 0f, 0f);
        creature2MoveStep = new Vector3(0.3f, 0f, 0f);
        playerMoveStep = new Vector3(0.1f, 0f, 0f);
    }

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.Play(); 
        }
        
        if (creature3 != null)
            creature3.SetActive(false);
            
        if (ball != null)
            ball.SetActive(false);
        
        creature2Script.OnSpeedChanged += OnCreatureSpeedChanged;
        
        if(StartMenu.hardmode == 0) 
            DestroyCreature1();
            
        StartCoroutine(CeilingAndFloorComeOut());
    }

    private IEnumerator CeilingAndFloorComeOut()
    {
        while (ceiling.transform.position.x >= 0.5f || floor.transform.position.x >= 0.5f)
        {
            ceiling.transform.position -= ceilingMoveStep;
            floor.transform.position -= floorMoveStep;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            audioVolume = Mathf.Clamp01(audioVolume + 0.02f);
            
            Color bgColor = background2.color;
            bgColor.a = alpha;
            background2.color = bgColor;
            
            audioSource.volume = audioVolume;
            
            yield return new WaitForSeconds(0.1f);
        }
        
        StartCoroutine(BossComeOut());
    }

    private IEnumerator BossComeOut()
    {
        while (creature2.transform.position.x > 7f)
        {
            creature2.transform.position -= creature2MoveStep;
            yield return new WaitForSeconds(0.1f);
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
                
            Vector3 pos = creature2.transform.position;
            creature2.transform.position = new Vector3(pos.x, 0f, pos.z);
            
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
        
        while (ceiling.transform.position.x > -20f || floor.transform.position.x > -20f)
        {
            ceiling.transform.position -= ceilingMoveStep;
            floor.transform.position -= floorMoveStep;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            
            // Reuse color objects to reduce GC
            Color bgColor = background3.color;
            bgColor.a = alpha;
            background3.color = bgColor;
            
            yield return new WaitForSeconds(0.1f);
        }
        
        if (ceiling != null)
            Destroy(ceiling);
            
        if (floor != null)
            Destroy(floor);

        creauture2Animator.SetTrigger("death");
        creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;

        alpha = 0f;
        StartCoroutine(PlayerMoveMiddle());
    }

    private IEnumerator PlayerMoveMiddle()
    {
        while (player.transform.position.x <= 0f)
        {
            player.transform.position += playerMoveStep;
            
            Color bgColor = background4.color;
            bgColor.a = alpha;
            background4.color = bgColor;

            alpha = Mathf.Clamp01(alpha + 0.02f);
            
            yield return new WaitForSeconds(0.1f);
        }
        
        if (creature3 != null)
            creature3.SetActive(true);
            
        StartCoroutine(CheckGoToLv2());
    }

    private IEnumerator CheckGoToLv2()
    {
        while (creature3!=null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StateManager.Instance.goToLv2();
    }
    
    void DestroyCreature1()
    {
        ConfigCreature1.canreuse = false;
    }
}