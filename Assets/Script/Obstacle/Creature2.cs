using UnityEngine;

public class Creature2 : MonoBehaviour
{
    [SerializeField] private Transform ball;  
    [SerializeField] private float maxY;    
    [SerializeField] private float minY;
    [SerializeField] private Animator animator;

    public int speed=0;
    public System.Action<int> OnSpeedChanged; 
    void Start()
    {
        ChooseHard(StartMenu.hardmode);
        if (ball == null) return;
    }
    public int Speed
    {
        get => speed;
        set
        {
            speed = value;
            OnSpeedChanged?.Invoke(speed);
            ChooseHard(StartMenu.hardmode);
        }
    }

    void Update()
    {
        if (ball == null) return;

        Vector3 targetPosition = new Vector3(transform.position.x, ball.position.y, transform.position.z);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position.x < -20f) 
        {
            Destroy(gameObject);
        }
    }
    private void ChooseHard(int hardmode)
    {
        switch (hardmode)
        {
            case 0:
                if (speed==0) speed=2;
                if (speed==3) animator.SetInteger("Phase", 1);
                if (speed==4) animator.SetInteger("Phase", 2);
                break;
            case 1:
                if (speed==0) speed=3;
                if (speed==4) animator.SetInteger("Phase", 1);
                if (speed==5) animator.SetInteger("Phase", 2);
                break;
            case 2:
                if (speed==0) speed=4;
                if (speed==5) animator.SetInteger("Phase", 1);
                if (speed==6) animator.SetInteger("Phase", 2);
                break;
            default:
                break;
        }
    }
    
}