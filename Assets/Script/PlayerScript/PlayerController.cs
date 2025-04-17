using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float strength;

    [SerializeField] protected InputAction InputActionMovement;

    protected Vector2 move;
    protected Rigidbody2D rigidbody2d;
    public static bool canMove = true;

    protected virtual void Start()
    {
        InputActionMovement.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        InputActionMovement.performed += _ => movement();
        InputActionMovement.canceled += ctx => move = Vector2.zero;
        PlayerBehavior.OnPlayerDied += DisableMovement;
    }

    protected virtual void OnDestroy()
    {
        PlayerBehavior.OnPlayerDied -= DisableMovement;
    }

    protected virtual void movement()
    {
        if (!canMove || rigidbody2d == null) return;

        Vector2 moveinput = InputActionMovement.ReadValue<Vector2>();
        rigidbody2d.linearVelocity = moveinput * strength;
    }

    protected virtual void DisableMovement()
    {
        canMove = false;
        rigidbody2d.linearVelocity = Vector2.zero;
    }
}