using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    [SerializeField] private Transform creature2; 
    [SerializeField] Creature2 creature2script;
    [SerializeField] private float SpeedIncrease;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchLeft();
        InvokeRepeating("CheckOut",0f,0.2f);
    }

    void LaunchLeft()
    {
        rb.transform.position = new Vector2(creature2.position.x-2, creature2.position.y);
        speed = 5f;
        Vector2 dir = new Vector2(-1, Random.Range(-0.5f, 0.5f)).normalized;
        rb.linearVelocity = dir * speed;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir = rb.linearVelocity;
        dir=FixGoStraight(dir);
        speed=speed+SpeedIncrease;       
        rb.linearVelocity = dir.normalized * speed;
    }
    void CheckOut()
    {
        if (rb.transform.position.x > 10f )
        {
            LaunchLeft();
            creature2script.Speed += 1;
            StateManager.Instance.Score += 5;
        }
        if (rb.transform.position.x<-10f)
        {
            StateManager.Instance.Score-=5;
            LaunchLeft();
        }
    }
    Vector2 FixGoStraight(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) < 2f)
        {
            dir.y = Random.Range(-15f, 15f); 
        }
         if (Mathf.Abs(dir.x) < 2f)
        {
            dir.x = Random.Range(-15f, 15f);
        }
       
        return dir;
    }
}