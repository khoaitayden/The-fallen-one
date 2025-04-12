using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float strength;
    [SerializeField] private InputAction InputActionMovement;

    Vector2 move;
    Rigidbody2D rigidbody2d;
    private bool canMove = true;

    void Start()
    {
        InputActionMovement.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        InputActionMovement.performed += _ => movement(); 
        InputActionMovement.canceled += ctx => move = Vector2.zero;

        PlayerBehavior.OnPlayerDied += DisableMovement;
    }

    void OnDestroy()
    {
        PlayerBehavior.OnPlayerDied -= DisableMovement;
    }

    void movement()
    {
        if (!canMove) return;

        Vector2 moveinput = InputActionMovement.ReadValue<Vector2>();
        rigidbody2d.linearVelocity = moveinput * strength;
    }

    void DisableMovement()
    {
        canMove = false;
        rigidbody2d.linearVelocity = Vector2.zero;
    }
    
}