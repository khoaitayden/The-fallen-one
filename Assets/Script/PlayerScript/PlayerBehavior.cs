using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private InputAction attackAction;
    [SerializeField] private InputAction FlyAction;
 //   [SerializeField] private InputAction DashAction;
    [SerializeField] private Animator animator; 
    [SerializeField] private float attackCooldown = 2.0f; 
    [SerializeField] private ObstacleCreature1SpawnScript obstaclecreature1;

    private bool canAttack = true; 
    private bool hitflycreature1=false;
    private List<GameObject> flycreatures = new List<GameObject>();
    public bool checkhitflycreature1
    {
        get { return hitflycreature1; }
    }
    private GameObject flycreature1;
    private Rigidbody2D rb;

    void Start()
    {
        InvokeRepeating(nameof(CheckEnemiesInRange), 0f, 0.2f);
    }
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        attackAction.Enable();
        attackAction.performed += _ => TryAttack(); 

        FlyAction.Enable(); 
        FlyAction.performed += _ => fly();

    //    DashAction.Enable(); 
    //    DashAction.performed += _ => dash(); 
    }

    void OnDisable()
    {
        attackAction.performed -= _ => TryAttack(); 
        attackAction.Disable();

        FlyAction.performed -= _ => fly(); 
        FlyAction.Disable(); 

    //    DashAction.performed -= _ => dash(); 
    //    DashAction.Disable();
    }


    void TryAttack()
    {
        if (!canAttack) return; 
        firstattack();
        hitattack();
        StartCoroutine(AttackCooldown());
    }
void hitattack()
{
    if (hitflycreature1)
    {
        for (int i = 0; i < flycreatures.Count; i++)
        {
            for (int j = 0; j < obstaclecreature1.CreaturePool.Count; j++)
            {
                if (obstaclecreature1.CreaturePool[j] == flycreatures[i])
                {
                    obstaclecreature1.ReuseCreature(j);
                }
            }
        }

        // Clear the list after handling all flycreatures
        flycreatures.Clear();
        hitflycreature1 = false;
    }
}
    void firstattack()
    {
        animator.SetTrigger("FirstAttack"); 
    }
    void fly()
    {
        animator.SetTrigger("Fly"); 
    }

    // Handles the attack cooldown
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown); 
        canAttack = true; 
    }


    public void SetAttackCooldown(float newCooldown)
    {
        attackCooldown = newCooldown;
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("flycreature1"))
    //     {
    //         {
    //             hitflycreature1=true;
    //             flycreature1=collision.gameObject;
    //         }
    //     }
    // }
    
        void CheckEnemiesInRange()
        {
            Vector2 origin = transform.position;
            float radius = 2.0f;
            int layerMask = LayerMask.GetMask("Enemy");

            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, layerMask);
            
            flycreatures.Clear();

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("flycreature1"))
                {
                    flycreatures.Add(hit.gameObject);
                }
            }

            hitflycreature1 = flycreatures.Count > 0;
            flycreature1 = hitflycreature1 ? flycreatures[0] : null;
        }
    void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        float radius = 2.0f;

        // Draw the detection area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
