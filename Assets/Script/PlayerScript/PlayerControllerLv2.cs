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

    if (Mathf.Abs(moveinput.x) > Mathf.Abs(moveinput.y))
    {
        spriteRenderer.sprite = moveinput.x > 0 ? spriteRight : spriteLeft;
    }
    else
    {
        spriteRenderer.sprite = moveinput.y > 0 ? spriteUp : spriteDown;
    }

    // Rotate the sprite to match direction (since all sprites are drawn facing up)
    float angle = Mathf.Atan2(moveinput.y, moveinput.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
}
void OnCollisionEnter2D(Collision2D collision)
{
    TetrisBlock piece = FindAnyObjectByType<TetrisSpawner>().activePiece;
    if (piece == null || piece.isLocked) return;
    foreach (ContactPoint2D contact in collision.contacts)
    {
        Vector2 normal = contact.normal;
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            if (normal.x > 0)
            {
                Debug.Log("Push Left");
                piece.TryMove(Vector3.left);
            }
            else
            {
                Debug.Log("Push Right");
                piece.TryMove(Vector3.right);
            }
        }
        else
        {
            if (normal.y > 0)
            {
                Debug.Log("Push Down");
                piece.TryMove(Vector3.down);
                piece.lastFallTime = Time.time;
            }
        }
    }
}
}
