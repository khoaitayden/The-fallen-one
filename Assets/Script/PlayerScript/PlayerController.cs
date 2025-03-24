using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float strength;
    [SerializeField] private InputAction InputActionMovement;
    Vector2 move;
    Rigidbody2D rigidbody2d;
    void Start()
    {
        InputActionMovement.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        InputActionMovement.performed += _ => movement(); 
        InputActionMovement.canceled += ctx => move = Vector2.zero;
    }
    void movement()
    {
        //rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpstrength);
        Vector2 moveinput=InputActionMovement.ReadValue<Vector2>();
        rigidbody2d.linearVelocity=moveinput*strength;

    }
}
