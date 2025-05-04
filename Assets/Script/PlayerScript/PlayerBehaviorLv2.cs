using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerBehaviorLv2 : MonoBehaviour
{

    [SerializeField] private float pushCooldown;
    [SerializeField] private float maxOutSideGridTime;
    [SerializeField] private Slider dangerSlider;
    [SerializeField] private AudioSource dyingSoundSource;
    private float lastPushTime = 0f;
    private Dictionary<Vector2, float> lastPushTimes = new();
    private Rigidbody2D rb;
    private float DangerMeter=0f;
    public static bool PlayerTracked ;

    void Start()
    {
        PlayerTracked = false;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            return;
        }
        PlayerBehavior.dyingSound = dyingSoundSource;
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
    void FixedUpdate()
    {
        float increaseRate = 0.5f;
        float decreaseRate = 0.3f;

        if (AreaChecker.isPlayerInsideTheGrid)
        {
            DangerMeter = Mathf.Clamp(DangerMeter - decreaseRate * Time.fixedDeltaTime, 0, maxOutSideGridTime);
        }
        else
        {
            DangerMeter = Mathf.Clamp(DangerMeter + increaseRate * Time.fixedDeltaTime, 0, maxOutSideGridTime);
            if (DangerMeter >= maxOutSideGridTime)
            {
                PlayerTracked=true;
            }
        }
        dangerSlider.value = DangerMeter / maxOutSideGridTime;
    }
}