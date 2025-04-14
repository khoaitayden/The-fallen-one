using UnityEngine;

public class Bossstage : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject creature2;
    [SerializeField] private GameObject ceiling;
    [SerializeField] private GameObject floor;
    [SerializeField] private Creature2 creature2Script;
    [SerializeField] private SpriteRenderer background2;
    [SerializeField] private SpriteRenderer background3;
    [SerializeField] private Animator creauture2Animator;
    private float alpha = 0f;
    void Start()
    {
        creature2Script.OnSpeedChanged += OnCreatureSpeedChanged;
        InvokeRepeating("ceilandfloorcomeout", 0f, 0.1f);
    }

    void ceilandfloorcomeout()
    {
        if (ceiling.transform.position.x < 0.5f && floor.transform.position.x < 0.5f)
        {
            CancelInvoke("ceilandfloorcomeout");
            InvokeRepeating("bosscomeout", 0f, 0.1f);
        }

        ceiling.transform.position = new Vector3(ceiling.transform.position.x - 0.5f, ceiling.transform.position.y, ceiling.transform.position.z);
        floor.transform.position = new Vector3(floor.transform.position.x - 0.5f, floor.transform.position.y, floor.transform.position.z);

        alpha = Mathf.Clamp01(alpha + 0.02f); 
        background2.color = new Color(background2.color.r, background2.color.g, background2.color.b, alpha);
    }
    void ceilandfloorgoaway()
    {
        if (ceiling.transform.position.x < -20f && floor.transform.position.x < -20f)
        {
            CancelInvoke("ceilandfloorgoaway");
            Destroy(ceiling);
            Destroy(floor);
            creauture2Animator.SetTrigger("death");
            creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        ceiling.transform.position = new Vector3(ceiling.transform.position.x - 0.5f, ceiling.transform.position.y, ceiling.transform.position.z);
        floor.transform.position = new Vector3(floor.transform.position.x - 0.5f, floor.transform.position.y, floor.transform.position.z);

        alpha = Mathf.Clamp01(alpha + 0.02f); 
        background3.color = new Color(background3.color.r, background3.color.g, background3.color.b, alpha);
    }

    void bosscomeout()
    {
        if (creature2.transform.position.x <= 7f)
        {
            CancelInvoke("bosscomeout");
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
            InvokeRepeating("ceilandfloorgoaway", 0f, 0.1f);
        }
    }
}