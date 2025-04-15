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
    [SerializeField] private Creature2 creature2Script;
    [SerializeField] private SpriteRenderer background2;
    [SerializeField] private SpriteRenderer background3;
    [SerializeField] private SpriteRenderer background4;
    [SerializeField] private Animator creauture2Animator;
    private float alpha = 0f;
    void Start()
    {
        creature2Script.OnSpeedChanged += OnCreatureSpeedChanged;
        InvokeRepeating("ceilAndFloorComeOut", 0f, 0.1f);
    }

    void ceilAndFloorComeOut()
    {
        if (ceiling.transform.position.x < 0.5f && floor.transform.position.x < 0.5f)
        {
            CancelInvoke("ceilAndFloorComeOut");
            InvokeRepeating("bossComeOut", 0f, 0.1f);
        }

        ceiling.transform.position = new Vector3(ceiling.transform.position.x - 0.5f, ceiling.transform.position.y, ceiling.transform.position.z);
        floor.transform.position = new Vector3(floor.transform.position.x - 0.5f, floor.transform.position.y, floor.transform.position.z);

        alpha = Mathf.Clamp01(alpha + 0.02f); 
        background2.color = new Color(background2.color.r, background2.color.g, background2.color.b, alpha);
    }
    void theEndCutScene()
    {
        ceiling.transform.position = new Vector3(ceiling.transform.position.x - 0.5f, ceiling.transform.position.y, ceiling.transform.position.z);
        floor.transform.position = new Vector3(floor.transform.position.x - 0.5f, floor.transform.position.y, floor.transform.position.z);

        alpha = Mathf.Clamp01(alpha + 0.02f); 
        background3.color = new Color(background3.color.r, background3.color.g, background3.color.b, alpha);
        if (ceiling.transform.position.x < -20f && floor.transform.position.x < -20f)
        {
            CancelInvoke("theEndCutScene");
            Destroy(ceiling);
            Destroy(floor);
            creauture2Animator.SetTrigger("death");
            creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;
            alpha=0;
            InvokeRepeating("playerMoveMiddle", 0f, 0.1f);
            
        }
    }

    void bossComeOut()
    {
        if (creature2.transform.position.x <= 7f)
        {
            CancelInvoke("bossComeOut");
            ball.SetActive(true);
        }

        creature2.transform.position = new Vector3(creature2.transform.position.x - 0.3f, creature2.transform.position.y, creature2.transform.position.z);
    }

    void OnCreatureSpeedChanged(int newSpeed)
    {
        if (newSpeed == 6) 
        {
            Destroy(ball);
            creature2.transform.position = new Vector3(creature2.transform.position.x, 0f, creature2.transform.position.z);
            alpha=0;
            InvokeRepeating("theEndCutScene", 0f, 0.1f);
        }
    }
    private void playerMoveMiddle()
    {
         
        if (player.transform.position.x <= 0f)
        {
            player.transform.position = new Vector3(player.transform.position.x + 0.1f, player.transform.position.y, player.transform.position.z);
            background4.color = new Color(background4.color.r, background4.color.g, background4.color.b, alpha);
            alpha = Mathf.Clamp01(alpha + 0.02f);
        }
        else
        {
            CancelInvoke("playerMoveMiddle");
            creature3.SetActive(true);
            InvokeRepeating("checkGoToLv2", 0f, 0.2f);
        }
    }
    private void checkGoToLv2()
    {
        if (player.transform.position.y <= -15f)
        {
            CancelInvoke("checkGoToLv2");
            StateManager.Instance.goToLv2();
        }
    }
}