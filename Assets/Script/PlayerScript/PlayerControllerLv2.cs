using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControllerLv2 : PlayerController
{
    [Header("Directional Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

protected override void movement()
{
    if (!canMove || rigidbody2d == null) return;

    Vector2 moveinput = InputActionMovement.ReadValue<Vector2>();
    rigidbody2d.linearVelocity = moveinput * strength;

    if (moveinput == Vector2.zero) return;

    // Switch sprite based on direction
    if (Mathf.Abs(moveinput.x) > Mathf.Abs(moveinput.y))
    {
        // Horizontal movement
        spriteRenderer.sprite = moveinput.x > 0 ? spriteRight : spriteLeft;
    }
    else
    {
        // Vertical movement
        spriteRenderer.sprite = moveinput.y > 0 ? spriteUp : spriteDown;
    }

    // Rotate the sprite to match direction (since all sprites are drawn facing up)
    float angle = Mathf.Atan2(moveinput.y, moveinput.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
}
}
