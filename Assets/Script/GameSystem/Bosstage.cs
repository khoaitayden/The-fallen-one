using UnityEngine;

public class Bossstage : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject creature2;
    [SerializeField] private GameObject ceiling;
    [SerializeField] private GameObject floor;
    [SerializeField] private Creature2 creature2Script;

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
    }
    void ceilandfloorgoaway()
    {
        if (ceiling.transform.position.x < -20f && floor.transform.position.x < -20f)
        {
            CancelInvoke("ceilandfloorgoaway");
            Destroy(ceiling);
            Destroy(floor);
            creature2.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        ceiling.transform.position = new Vector3(ceiling.transform.position.x - 0.5f, ceiling.transform.position.y, ceiling.transform.position.z);
        floor.transform.position = new Vector3(floor.transform.position.x - 0.5f, floor.transform.position.y, floor.transform.position.z);
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
        if (newSpeed == 4) 
        {
            Destroy(ball);
            creature2.transform.position = new Vector3(creature2.transform.position.x, 0f, creature2.transform.position.z);
            InvokeRepeating("ceilandfloorgoaway", 0f, 0.1f);
        }
    }
}