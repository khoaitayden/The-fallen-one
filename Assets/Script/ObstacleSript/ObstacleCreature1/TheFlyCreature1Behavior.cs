using UnityEngine;

public class TheFlyCreature1Behavior : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ObstacleCreature1SpawnScript obstaclecreature1;
    void Start()
    {
        obstaclecreature1 = FindObjectOfType<ObstacleCreature1SpawnScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("Speed",obstaclecreature1.Speed/5);
        animator.speed = obstaclecreature1.Speed/3f; // Adjust animation speed
    }
}
