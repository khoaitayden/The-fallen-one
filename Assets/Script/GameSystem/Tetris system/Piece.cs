using UnityEngine;
using System.Collections;
public class Piece : MonoBehaviour
{
    // --------------------------- Public Properties ---------------------------

    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Board board { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }
    public bool IsLocked => isLocked;
    public void PushFromPlayer(Vector2Int direction, bool resetLockTimer = false)
    {
        externalPush = new ExternalPush { direction = direction, resetLockTimer = resetLockTimer };
    }

    // --------------------------- Private Fields ---------------------------

    public Vector3Int[] cellOffsets;
    [SerializeField]private float stepDelay = 1f;
    [SerializeField]private float lockDelay = 0.5f;
    private float stepTime;
    private float lockTime;

    private bool isLocked = false;
    private struct ExternalPush
    {
        public Vector2Int direction;
        public bool resetLockTimer;
    }
    private ExternalPush? externalPush = null;

    private float pushCooldown = 0.05f;
    private float lastPushTime = 0f;

    // --------------------------- Initialization ---------------------------
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

        int count = data.cells.Length;
        cells = new Vector3Int[count];
        cellOffsets = new Vector3Int[count];

        SetBaseOffsets();
        UpdateCellPositions();

        stepTime = Time.time + stepDelay;
        lockTime = Time.time + lockDelay;
        isLocked = false;
    }

    private void SetBaseOffsets()
    {
        for (int i = 0; i < data.cells.Length; i++)
        {
            Vector2 baseCell = data.cells[i];
            cellOffsets[i] = new Vector3Int(Mathf.RoundToInt(baseCell.x), Mathf.RoundToInt(baseCell.y), 0);
        }
    }

    private void UpdateCellPositions()
    {
        for (int i = 0; i < cellOffsets.Length; i++)
        {
            cells[i] = position + cellOffsets[i];
        }
    }

    // --------------------------- Unity Update ---------------------------

    private void Update()
    {
        board.Clear(this);

    if (!isLocked)
    {
        if (externalPush.HasValue && Time.time - lastPushTime >= pushCooldown)
        {
            TryMove(externalPush.Value.direction, externalPush.Value.resetLockTimer);
            externalPush = null;
            lastPushTime = Time.time;
        }

        HandleInput();
        HandleAutoStep();
    }

        board.Set(this);
    }

    // --------------------------- Input Handling ---------------------------

    private void HandleInput()
    {
        //if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector2Int.left);
        //if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector2Int.right);
        //if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector2Int.down, true); // Reset lock timer on soft drop
        if (Input.GetKeyDown(KeyCode.LeftShift)) Rotate(-1);
        if (Input.GetKeyDown(KeyCode.LeftControl)) Rotate(1);
        if (Input.GetKeyDown(KeyCode.Space)) HardDrop();
    }

    // --------------------------- Step-Down and Lock ---------------------------

    private void HandleAutoStep()
    {
        if (Time.time >= stepTime)
        {
            StepDown();
        }
    }

    private void StepDown()
    {
        if (!TryMove(Vector2Int.down))
        {
            if (Time.time >= lockTime)
            {
                Lock();
            }
        }
        else
        {
            lockTime = Time.time + lockDelay;
        }

        stepTime = Time.time + stepDelay;
    }

    private void Lock()
    {
        StartCoroutine(LockRoutine());
    }

    private IEnumerator LockRoutine()
    {
        board.Set(this);
        yield return board.ClearLinesCoroutine(); 
        board.SpawnPiece(); 
        isLocked = true;
    }

    // --------------------------- Movement ---------------------------

    public bool TryMove(Vector2Int direction, bool resetLockTimer = false)
    {
        Debug.Log($"Trying to move {direction}");
        Vector3Int newPosition = position + new Vector3Int(direction.x, direction.y, 0);
        if (board.IsValidPosition(this, newPosition))
        {
            position = newPosition;
            UpdateCellPositions();

            if (resetLockTimer)
                lockTime = Time.time + lockDelay;

            return true;
        }

        return false;
    }

    private void HardDrop()
    {
        while (TryMove(Vector2Int.down)) { }

        Lock();
    }

    // --------------------------- Rotation with Wall Kicks ---------------------------

    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        Vector3Int originalPosition = position;
        Vector3Int[] originalOffsets = (Vector3Int[])cellOffsets.Clone();
        Vector3Int[] originalCells = (Vector3Int[])cells.Clone();

        rotationIndex = Wrap(rotationIndex + direction, 0, 4);

        Vector3 pivotOffset = (data.tetromino == Tetromino.I || data.tetromino == Tetromino.O)
            ? new Vector3(0.5f, 0.5f, 0f)
            : Vector3.zero;

        Vector3Int[] rotatedOffsets = GetRotatedOffsets(direction, pivotOffset);
        Vector3Int[] newCells = GetRotatedWorldCells(rotatedOffsets);

        if (board.IsValidPosition(position, rotatedOffsets))
        {
            cellOffsets = rotatedOffsets;
            cells = newCells;
            lockTime = Time.time + lockDelay;
        }
        else if (TestWallKicks(originalRotation, direction, rotatedOffsets))
        {
            cellOffsets = rotatedOffsets;
            cells = GetRotatedWorldCells(cellOffsets);
            lockTime = Time.time + lockDelay;
        }
        else
        {
            rotationIndex = originalRotation;
            position = originalPosition;
            cellOffsets = originalOffsets;
            cells = originalCells;
        }
    }

    private Vector3Int[] GetRotatedOffsets(int direction, Vector3 pivotOffset)
    {
        Vector3Int[] rotated = new Vector3Int[cellOffsets.Length];

        for (int i = 0; i < cellOffsets.Length; i++)
        {
            Vector3 offset = cellOffsets[i] - pivotOffset;

            float x = offset.x * Data.RotationMatrix[0] * direction + offset.y * Data.RotationMatrix[1] * direction;
            float y = offset.x * Data.RotationMatrix[2] * direction + offset.y * Data.RotationMatrix[3] * direction;

            offset = new Vector3(x, y, 0f) + pivotOffset;
            rotated[i] = new Vector3Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y), 0);
        }

        return rotated;
    }

    private Vector3Int[] GetRotatedWorldCells(Vector3Int[] rotatedOffsets)
    {
        Vector3Int[] worldCells = new Vector3Int[rotatedOffsets.Length];
        for (int i = 0; i < rotatedOffsets.Length; i++)
        {
            worldCells[i] = position + rotatedOffsets[i];
        }
        return worldCells;
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection, Vector3Int[] rotatedOffsets)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];
            Vector3Int newPosition = position + new Vector3Int(translation.x, translation.y, 0);

            if (board.IsValidPosition(newPosition, rotatedOffsets))
            {
                position = newPosition;
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int index = rotationIndex * 2;
        if (rotationDirection < 0)
        {
            index--;
        }
        return Wrap(index, 0, data.wallKicks.GetLength(0));
    }

    // --------------------------- Utility ---------------------------

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
            return max - (min - input) % (max - min);
        else
            return min + (input - min) % (max - min);
    }
}