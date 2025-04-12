using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public static event System.Action OnPlayerDied;
    [Header("Attack 1 Settings")]
    [SerializeField] private float checkInterval = 0.3f;
    [SerializeField] private float detectionRadius = 1.0f;
    [SerializeField] private float attack1Cooldown;

    [Header("Input References")]
    [SerializeField] private InputAction attack1;
    [SerializeField] private InputAction fly;

    [Header("Additional References")]
    [SerializeField] private ConfigCreature1 ConfigCreature1;
    [SerializeField] private Animator animator;

    [Header("UI References")]
    [SerializeField] private Slider attack1CooldownSlider;
    [Header("Effect")]
    [SerializeField] private GameObject FirstAttackKillEffect;


    private int enemyLayerMask;
    private bool canAttack1 = true;
    private bool isDead = false;
    private List<GameObject> EnemyInAttack1Range = new List<GameObject>();

    void Start()
    {
        enemyLayerMask = LayerMask.GetMask("Enemy");
        InvokeRepeating(nameof(CheckEnemiesInRange), 0f, checkInterval);
        InvokeRepeating(nameof(CheckFalloff), 0f, checkInterval);
        OnPlayerDied += DisablePlayer;
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
        attack1.performed -= TryAttack1;
        fly.performed -= Fly;
        attack1.Disable();
        fly.Disable();
    }

    void OnDestroy()
    {
        OnPlayerDied -= DisablePlayer;
    }

    void Fly(InputAction.CallbackContext context)
    {
        if (isDead) return;
        if (context.performed)
        {
            animator?.SetTrigger("Fly");
        }
    }

    void TryAttack1(InputAction.CallbackContext context)
    {
        if (isDead || !canAttack1) return;
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
                 if (FirstAttackKillEffect != null)
                {
                GameObject effect = Instantiate(FirstAttackKillEffect, enemy.transform.position, Quaternion.identity);
                Destroy(effect, 0.4f);
                }
                float randomy = Random.Range(ConfigCreature1.minHeight, ConfigCreature1.maxHeight);
                float randomx = Random.Range(ConfigCreature1.minSpacing, ConfigCreature1.maxSpacing);
                Vector3 randomPosition = new Vector3(randomx, randomy, 0);
                enemy.transform.position = randomPosition;
            }
        }
    }

    void CheckEnemiesInRange()
    {
        if (isDead) return;

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

    IEnumerator Attack1Cooldown()
    {
        canAttack1 = false;
        float elapsed = 0f;
        attack1CooldownSlider.value = 1f;

        while (elapsed < attack1Cooldown)
        {
            elapsed += Time.deltaTime;
            attack1CooldownSlider.value = 1f - (elapsed / attack1Cooldown);
            yield return null;
        }

        attack1CooldownSlider.value = 0f;
        canAttack1 = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("ball"))
        {
            animator?.SetTrigger("Hitball");
        }
    }

    void DisablePlayer()
    {
        isDead = true;
        attack1.Disable();
        fly.Disable();
        StopAllCoroutines();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    public static void TriggerPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
    void CheckFalloff()
    {
        if (transform.position.y < -6f||transform.position.y > 6f)
        {
            TriggerPlayerDied();
        }
    }
}