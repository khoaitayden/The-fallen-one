using UnityEngine;
using UnityEngine.SceneManagement;
public class Creature3 : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;// Player must go above this Y value to trigger chase
    
    private bool isChasing = false;
    private Rigidbody2D rb;
    Scene currentScene;
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
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);

            if (rb.linearVelocity.magnitude >speed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speed;
            }
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