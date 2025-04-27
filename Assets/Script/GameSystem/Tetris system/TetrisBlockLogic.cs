using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Transactions;
public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    public float moveDelay = 0.1f;
    public static float fallTime = 2f;
    public static int height = 10;
    public static int wide = 15;
    public int BlockIndex; 

    [Header("Landing Settings")]
    public float landingDelay = 1f;
    private float landingTimer;
    private bool hasLanded;
    public float lastMoveTime;
    public float lastFallTime;

    public bool isLocked;

    private TetrisSpawner spawner;
    public static Transform[,] grid = new Transform[wide, height];

    private void Start()
    {
        spawner = FindFirstObjectByType<TetrisSpawner>();
    }

    private void Update()
    {
        if (isLocked) return;

        HandleMovement();
        HandleFalling();
    }
    void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Counter-clockwise
        {
            TryRotateAndWallKick(-90f);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) // Clockwise
        {
            TryRotateAndWallKick(90f);
        }
    }
    bool TryRotateAndWallKick(float angle)
    {
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // Rotate first
        transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, angle);

        // Wall kick offsets
        Vector2[] wallKickOffsets = new Vector2[]
        {
            Vector2.zero,
            Vector2.right,
            Vector2.left,
            Vector2.up,
            Vector2.down
        };

        foreach (Vector2 offset in wallKickOffsets)
        {
            transform.position = originalPosition + new Vector3(offset.x, offset.y, 0f);
            if (IsInsideBounds())
                return true;
        }

        // Revert if none of the offsets work
        transform.rotation = originalRotation;
        transform.position = originalPosition;
        return false;
    }

    void HandleFalling()
    {
        if (Time.time - lastFallTime >= fallTime)
        {
            TryMove(Vector3.down);
            lastFallTime = Time.time;
        }

        if (hasLanded)
        {
            if (IsStillGrounded())
            {
                landingTimer += Time.deltaTime;
                if (landingTimer >= landingDelay)
                {
                    LockPiece();
                    AddToGrid();
                    CheckForLines();
                }
            }
            else
            {
                hasLanded = false;
                landingTimer = 0;
            }
        }
    }
    bool IsStillGrounded()
    {
        foreach (Transform child in transform)
        {
            Vector2 checkPos = child.position + Vector3.down;
            int x = Mathf.RoundToInt(checkPos.x);
            int y = Mathf.RoundToInt(checkPos.y);

            if (y < 0 || x < 0 || x >= wide) return true;

            if (y < height && grid[x, y] != null)
            {
                return true;
            }
        }
        return false;
    }

    void StartLandingCountdown()
    {
        hasLanded = true;
        landingTimer = 0;
    }

    void LockPiece()
    {
        isLocked = true;
        this.enabled = false;

        AddToGrid();
        CheckGameOver();

        if (spawner != null)
        {
            spawner.SpawnNextPiece();
        }
        else
        {
            Debug.LogWarning("Spawner not found!");
        }
    }
    public void CheckGameOver()
    {
        for (int x = 0; x < wide; x++)
        {
            int topY = height-1;
            if (grid[x, topY] != null)
            {
                PlayerBehavior.TriggerPlayerDied();
                return;
            }
        }
    }
    public bool TryMove(Vector3 direction)
    {
        if (isLocked) return false;
        transform.position += direction;

        if (!IsInsideBounds())
        {
            transform.position -= direction;
            return false;
        }
        return true;
    }

    bool IsInsideBounds()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = child.position;
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);

            if (x < 0 || x >= wide|| y >= height)
            {
                return false;
            }
            if (y < 0)
            {
                StartLandingCountdown();
                return false;
            }
            if (grid[x,y] != null)
            {
                StartLandingCountdown();
                return false;
            }
        }
        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = child.position;
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);

            if (x >= 0 && x < wide && y >= 0 && y < height)
            {
                grid[x, y] = child;
            }
        }
    }

    void CheckForLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsFullLine(y))
            {
                StartCoroutine(AnimateAndClearLine(y));
                StateMangagerLv2.Instance.IncreaseScore(10);
                return;
            }
        }
    }

    bool IsFullLine(int y)
    {
        for (int x = 0; x < wide; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator AnimateAndClearLine(int y)
    {
        float flashDuration = 0.2f;
        float fadeDuration = 0.3f;

        // Flash
        for (int x = 0; x < wide; x++)
        {
            if (grid[x, y] != null)
            {
                SpriteRenderer sr = grid[x, y].GetComponent<SpriteRenderer>();
                if (sr != null) sr.color = Color.white;
            }
        }
        yield return new WaitForSeconds(flashDuration);

        // Fade
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            for (int x = 0; x < wide; x++)
            {
                if (grid[x, y] != null)
                {
                    SpriteRenderer sr = grid[x, y].GetComponent<SpriteRenderer>();
                    if (sr != null) sr.color = new Color(1f, 1f, 1f, alpha);
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Destroy line
        for (int x = 0; x < wide; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }

        // Move everything above down
        for (int i = y + 1; i < height; i++)
        {
            for (int x = 0; x < wide; x++)
            {
                if (grid[x, i] != null)
                {
                    grid[x, i].position += Vector3.down;
                    grid[x, i - 1] = grid[x, i];
                    grid[x, i] = null;
                }
            }
        }
        yield return null;
        CheckForLines();
    }
}