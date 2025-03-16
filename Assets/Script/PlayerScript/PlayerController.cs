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
    }
    void movement()
    {
        //rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpstrength);
        Debug.Log(InputActionMovement.ReadValue<Vector2>());
        Vector2 moveinput=InputActionMovement.ReadValue<Vector2>();
        rigidbody2d.linearVelocity=moveinput*strength;

    }
}
