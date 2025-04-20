using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    public float moveDelay = 0.1f;
    public static float fallTime = 1f;
    public static int height = 10;
    public static int wide = 15;

    [Header("Landing Settings")]
    public float landingDelay = 1f; // Time after landing before locking/spawning
    private float landingTimer;
    private bool hasLanded;
    public float lastMoveTime;
    public float lastFallTime;

    public bool isLocked;

    private TetrisSpawner spawner;
    private static Transform[,] grid = new Transform[wide, height];

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
        // if (Time.time - lastMoveTime > moveDelay)
        // {
        //     if (Input.GetKey(KeyCode.LeftArrow))
        //     {
        //         TryMove(Vector3.left);
        //         lastMoveTime = Time.time;
        //     }
        //     else if (Input.GetKey(KeyCode.RightArrow))
        //     {
        //         TryMove(Vector3.right);
        //         lastMoveTime = Time.time;
        //     }
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90f);

            if (!IsInsideBounds())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90f);
            }
        }

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     TryMove(Vector3.down);
        //     lastFallTime = Time.time;
        // }
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
            landingTimer += Time.deltaTime;
            if (landingTimer >= landingDelay)
            {
                LockPiece();
                AddToGrid();
                CheckForLines();
            }
        }
    }

    void StartLandingCountdown()
    {
        hasLanded = true;
        landingTimer = 0;
    }

    void LockPiece()
    {
        isLocked = true;
        Debug.Log("Piece locked!");
        this.enabled = false;
        // TODO: Add to grid, clear lines, etc.

        if (spawner != null)
        {
            spawner.SpawnNextPiece();
        }
        else
        {
            Debug.LogWarning("Spawner not found!");
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
    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = child.position;
            int x= Mathf.RoundToInt(pos.x);
            int y= Mathf.RoundToInt(pos.y);
            grid[x,y] = child;
        }
    }
    void CheckForLines()
    {
        for (int y = 0; y < height; y++)
        {
            bool isFullLine = true;
            for (int x = 0; x < wide; x++)
            {
                if (grid[x, y] == null)
                {
                    isFullLine = false;
                    break;
                }
            }

            if (isFullLine)
            {
                StartCoroutine(AnimateAndClearLine(y));
                return;
            }
        }
    }
    IEnumerator AnimateAndClearLine(int y)
    {
        float flashDuration = 0.2f;
        float fadeDuration = 0.3f;
    //Flash
        for (int x = 0; x < wide; x++)
        {
            if (grid[x, y] != null)
            {
                SpriteRenderer sr = grid[x, y].GetComponent<SpriteRenderer>();
                if (sr != null) sr.color = Color.white;
            }
        }

        yield return new WaitForSeconds(flashDuration);

        //Fade out
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

        // Destroy
        for (int x = 0; x < wide; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }

        // Move lines
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
        CheckForLines();
    }
}