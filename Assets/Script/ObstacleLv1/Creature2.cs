using UnityEngine;

public class Creature2 : MonoBehaviour
{
    [SerializeField] private Transform ball;  
    [SerializeField] private int speed;   
    [SerializeField] private float maxY ;    
    [SerializeField] private float minY;
    [SerializeField] private Animator animator;
    public int Speed
    {
        get { return speed; }
        set { speed = value;}
    }
    void Update()
    {
        if (ball == null) return;

        Vector3 targetPosition = new Vector3(transform.position.x, ball.position.y, transform.position.z);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}