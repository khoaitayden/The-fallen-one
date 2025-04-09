using UnityEngine;

public class BotPaddle : MonoBehaviour
{
    [SerializeField] private Transform ball;  
    [SerializeField] private float speed = 5f;   
    [SerializeField] private float maxY ;    // Limit for paddle movement (optional)
    [SerializeField] private float minY;   // Limit for paddle movement (optional)

    private void Update()
    {
        if (ball == null) return;

        Vector3 targetPosition = new Vector3(transform.position.x, ball.position.y, transform.position.z);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}