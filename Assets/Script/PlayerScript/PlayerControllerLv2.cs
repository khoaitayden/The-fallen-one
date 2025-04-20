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
    Piece piece = FindAnyObjectByType<Piece>();
    if(collision.gameObject.CompareTag("Locked"))
    {
        Debug.Log("Player collided with locked piece!");
    }
    if (piece == null || piece.IsLocked||collision.gameObject.CompareTag("Locked")) return; 
    foreach (ContactPoint2D contact in collision.contacts)
    {
        if (collision.gameObject.CompareTag("Locked")) break;
        Vector2 normal = contact.normal;
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            if (normal.x > 0)
            {
                //Debug.Log("Push Left");
                piece.PushFromPlayer(Vector2Int.left);
            }
            else
            {
                //Debug.Log("Push Right");
                piece.PushFromPlayer(Vector2Int.right);
            }
        }
        else
        {
            if (normal.y > 0)
            {
                //Debug.Log("Push Down");
                piece.PushFromPlayer(Vector2Int.down, true);
            }
        }
    }
}
}
