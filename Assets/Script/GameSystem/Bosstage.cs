using UnityEngine;
using System.Collections;

public class Bossstage : MonoBehaviour
{
    // === References ===
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject creature2;
    [SerializeField] private GameObject creature3;
    [SerializeField] private GameObject ceiling;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject player;

    [SerializeField] private Creature2 creature2Script;
    [SerializeField] private SpriteRenderer background2;
    [SerializeField] private SpriteRenderer background3;
    [SerializeField] private SpriteRenderer background4;
    [SerializeField] private Animator creauture2Animator;

    // === State ===
    private float alpha = 0f;
    private int hardset;
    void Start()
    {
        creature2Script.OnSpeedChanged += OnCreatureSpeedChanged;
        if(StartMenu.hardmode==0) DestroyCreature1();
        InvokeRepeating("ceilAndFloorComeOut", 0f, 0.1f);
    }

    // === Phase 1: Ceiling and Floor enter ===
    void ceilAndFloorComeOut()
    {
        if (ceiling.transform.position.x < 0.5f && floor.transform.position.x < 0.5f)
        {
            CancelInvoke("ceilAndFloorComeOut");
            InvokeRepeating("bossComeOut", 0f, 0.1f);
        }

        ceiling.transform.position -= new Vector3(0.5f, 0f, 0f);
        floor.transform.position   -= new Vector3(0.5f, 0f, 0f);

        alpha = Mathf.Clamp01(alpha + 0.02f);
        background2.color = new Color(
            background2.color.r,
            background2.color.g,
            background2.color.b,
            alpha
        );
    }

    // === Phase 2: Boss comes in ===
    void bossComeOut()
    {
        if (creature2.transform.position.x <= 7f)
        {
            CancelInvoke("bossComeOut");
            ball.SetActive(true);
        }

        creature2.transform.position -= new Vector3(0.3f, 0f, 0f);
    }

    // === Phase 3: On creature speed trigger ===
    void OnCreatureSpeedChanged(int newSpeed)
    {
        if (newSpeed == ChooseHard(StartMenu.hardmode))
        {
            Destroy(ball);
            creature2.transform.position = new Vector3(creature2.transform.position.x, 0f, creature2.transform.position.z);
            alpha = 0f;
            InvokeRepeating("theEndCutScene", 0f, 0.1f);
        }
    }
    private int ChooseHard(int hardmode)
    {
        switch (hardmode)
        {
            case 0:
                return 5;
            case 1:
                return 6;
            case 2:
                return 7;
            default:
                return 0;
        }
    }

    // === Phase 4: End cutscene ===
    void theEndCutScene()
    {
        DestroyCreature1();
        ceiling.transform.position -= new Vector3(0.5f, 0f, 0f);
        floor.transform.position   -= new Vector3(0.5f, 0f, 0f);

        alpha = Mathf.Clamp01(alpha + 0.02f);
        background3.color = new Color(
            background3.color.r,
            background3.color.g,
            background3.color.b,
            alpha
        );

        if (ceiling.transform.position.x < -20f && floor.transform.position.x < -20f)
        {
            CancelInvoke("theEndCutScene");
            Destroy(ceiling);
            Destroy(floor);

            creauture2Animator.SetTrigger("death");
            creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;

            alpha = 0f;
            InvokeRepeating("playerMoveMiddle", 0f, 0.1f);
        }
    }

    // === Phase 5: Player moves to center ===
    private void playerMoveMiddle()
    {
        if (player.transform.position.x <= 0f)
        {
            player.transform.position += new Vector3(0.1f, 0f, 0f);
            background4.color = new Color(
                background4.color.r,
                background4.color.g,
                background4.color.b,
                alpha
            );

            alpha = Mathf.Clamp01(alpha + 0.02f);
        }
        else
        {
            CancelInvoke("playerMoveMiddle");
            creature3.SetActive(true);
            InvokeRepeating("checkGoToLv2", 0f, 0.2f);
        }
    }

    // === Phase 6: Check to enter next level ===
    private void checkGoToLv2()
    {
        if (player.transform.position.y <= -15f)
        {
            CancelInvoke("checkGoToLv2");
            StateManager.Instance.goToLv2();
        }
    }
    void DestroyCreature1()
    {
        ConfigCreature1.canreuse = false;
    }
}