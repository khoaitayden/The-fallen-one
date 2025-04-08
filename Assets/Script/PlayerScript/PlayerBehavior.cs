using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float checkInterval = 0.3f;
    [SerializeField] private float detectionRadius = 1.0f;
    [SerializeField] private InputAction attack1;
    [SerializeField] private InputAction fly;
    [SerializeField] private ConfigCreature1 ConfigCreature1;
    [SerializeField] private Animator animator;
    [SerializeField] private float attack1Cooldown = 1f;
    private int enemyLayerMask;
    private bool canAttack1 = true;
    private List<GameObject> EnemyInAttack1Range = new List<GameObject>();

    

    void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        InvokeRepeating(nameof(CheckEnemiesInRange), 0f, checkInterval);
    }
    void OnEnable()
    {
        attack1.Enable();
        attack1.performed += TryAttack1;
        fly.Enable();
        fly.performed += Fly;
    }
    void OnDisable()
    {
        attack1.Disable();
        attack1.performed -= TryAttack1;
        fly.Disable();
        fly.performed -= Fly;
    }
    void Fly(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator?.SetTrigger("Fly");
        }
    }
    void TryAttack1(InputAction.CallbackContext context)
    {
        if (!canAttack1) return;
        Attack(EnemyInAttack1Range);
        StartCoroutine(Attack1Cooldown());
    }
    void Attack(List<GameObject> enemies)
    {
        animator?.SetTrigger("FirstAttack");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.CompareTag("flycreature1"))
            {
                float randomy = Random.Range(ConfigCreature1.minHeight, ConfigCreature1.maxHeight);
                float randomx = Random.Range(ConfigCreature1.minSpacing, ConfigCreature1.maxSpacing);
                Vector3 randomPosition = new Vector3(randomx, randomy, 0);
                enemy.transform.position = randomPosition;
            }
        }
    }
    void CheckEnemiesInRange()
    {
        EnemyInAttack1Range.Clear();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("flycreature1"))
            {
                EnemyInAttack1Range.Add(hit.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    IEnumerator Attack1Cooldown()
    {
        canAttack1 = false;
        yield return new WaitForSeconds(attack1Cooldown);
        canAttack1 = true;
    }
}