using UnityEngine;
using UnityEngine.SceneManagement;
public class Creature3 : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float activationHeight ; // Player must go above this Y value to trigger chase

    private bool isChasing = false;
    Scene currentScene;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        if (player == null) return;
        if (!isChasing && player.position.y > activationHeight||currentScene.name =="level1")
        {
            isChasing = true;
        }
        if (isChasing)
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = player.position;

            transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
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