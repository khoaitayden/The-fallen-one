using UnityEngine;

public class Creature3 : MonoBehaviour
{
    [SerializeField] private Transform player;   
    [SerializeField] private float speed = 3f;

    void Update()
    {
        if (player == null) return;

        Vector2 currentPos = transform.position;
        Vector2 targetPos = player.position;

        transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
