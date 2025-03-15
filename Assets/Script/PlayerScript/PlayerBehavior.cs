using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private InputAction attackAction; // Input action for attacking
    //[SerializeField] private InputAction FlyAction; // Input action for flying
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private float attackCooldown = 1.5f; // Cooldown time between attacks

    private bool canAttack = true; // Boolean to track if the player can attack

    void OnEnable()
    {
        attackAction.Enable(); // Enable the input action
        attackAction.performed += _ => TryAttack(); // Bind the attack function to the input action
        //FlyAction.Enable(); // Enable the input action
        //FlyAction.performed += _ => TryAttack(); // Bind the attack function to the input action
    }

    void OnDisable()
    {
        attackAction.performed -= _ => TryAttack(); // Unbind the attack function to prevent memory leaks
        attackAction.Disable(); // Disable the input action
        //FlyAction.performed -= _ => TryAttack(); // Unbind the attack function to prevent memory leaks
        //FlyAction.Disable(); // Disable the input action
    }

    // Attempt to attack if not on cooldown
    void TryAttack()
    {
        if (!canAttack) return; // If the player is on cooldown, ignore the attack
        firstattack(); // Trigger the attack animation
        StartCoroutine(AttackCooldown()); // Start cooldown coroutine
    }

    // Triggers the attack animation (now using a trigger)
    void firstattack()
    {
        animator.SetTrigger("FirstAttack"); // Use a trigger to play animation once
    }

    // Handles the attack cooldown
    IEnumerator AttackCooldown()
    {
        canAttack = false; // Prevent further attacks during cooldown
        yield return new WaitForSeconds(attackCooldown); // Wait for cooldown duration
        canAttack = true; // Allow attacking again
    }

    // Allows changing the attack cooldown dynamically
    public void SetAttackCooldown(float newCooldown)
    {
        attackCooldown = newCooldown;
    }
}
