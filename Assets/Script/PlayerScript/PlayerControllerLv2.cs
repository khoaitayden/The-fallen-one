using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class PlayerControllerLv2 : PlayerController
{
    [Header("Directional Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;
    private SpriteRenderer spriteRenderer;
    [Header("Push Settings")]
    [SerializeField] private float pushCooldown; // how often the player can push
    private float lastPushTime = 0f;
    private Dictionary<Vector2, float> lastPushTimes = new();
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canMove=true;
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
        if (!collision.transform.IsChildOf(piece.transform)) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal.normalized;
            Vector2 direction = Vector2.zero;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                direction = normal.x > 0 ? Vector2.left : Vector2.right;
            }
            else
            {
                if (normal.y > 0)
                    direction = Vector2.down;
            }

            if (direction == Vector2.zero) continue;

            // Check cooldown
            if (!lastPushTimes.ContainsKey(direction))
            {
                lastPushTimes[direction] = -Mathf.Infinity;
            }

            if (Time.time - lastPushTimes[direction] < pushCooldown)
                continue;

            // Apply push
            piece.TryMove(direction);
            if (direction == Vector2.down)
            {
                piece.lastFallTime = Time.time;
            }

            lastPushTimes[direction] = Time.time;
        }
    }
}
