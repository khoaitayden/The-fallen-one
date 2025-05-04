using UnityEngine;
using UnityEngine.SceneManagement;

public class Creature3 : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float slowingDistance = 2f; 
    [SerializeField] private float stoppingDistance = 0.5f; 
    
    private bool isChasing = false;
    private Rigidbody2D rb;
    private Scene currentScene;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (player == null) return;
        if (!isChasing && (player.transform.position.y > 11.5) && PlayerBehaviorLv2.PlayerTracked || currentScene.name == "level1")
        {
            isChasing = true;
        }
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float targetSpeed = speed;
            if (distanceToPlayer < slowingDistance)
            {
                targetSpeed = Mathf.Lerp(0, speed, distanceToPlayer / slowingDistance);
            }
            Vector2 targetVelocity = direction * targetSpeed;
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 5f);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * 3f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentScene = SceneManager.GetActiveScene();
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); 
            if (currentScene.name == "level2")
            {
                PlayerBehavior.TriggerPlayerDied();
            }
        }
    }
}