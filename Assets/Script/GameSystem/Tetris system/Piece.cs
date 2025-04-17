using UnityEngine;

public class Piece : MonoBehaviour
{
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Board board { get; private set; }

    public Vector3Int[] cells { get; private set; }         // Final positions on the board
    private Vector3Int[] cellOffsets;                       // Offsets from pivot
    public int rotationIndex { get; private set; }

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
    }

    private void Update()
    {
        board.Clear(this);

        HandleMovementInput();
        HandleRotationInput();
        HandleDropInput();

        board.Set(this);
    }

    // --------------------------- Input Handling ---------------------------

    private void HandleMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector2Int.right);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector2Int.down);
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) Rotate(-1);
        if (Input.GetKeyDown(KeyCode.LeftControl)) Rotate(1);
    }

    private void HandleDropInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) HardDrop();
    }

    // --------------------------- Movement ---------------------------

    private bool Move(Vector2Int direction)
    {
        Vector3Int newPosition = position + new Vector3Int(direction.x, direction.y, 0);
        if (board.IsValidPosition(this, newPosition))
        {
            position = newPosition;
            UpdateCellPositions();
            return true;
        }
        return false;
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down)) { }
    }

    // --------------------------- Rotation ---------------------------

    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);

        Vector3 pivotOffset = (data.tetromino == Tetromino.I || data.tetromino == Tetromino.O) 
            ? new Vector3(0.5f, 0.5f, 0f) 
            : Vector3.zero;

        Vector3Int[] rotatedOffsets = GetRotatedOffsets(direction, pivotOffset);
        Vector3Int[] newCells = GetRotatedWorldCells(rotatedOffsets);

        if (board.IsValidPosition(this, position))
        {
            cellOffsets = rotatedOffsets;
            cells = newCells;
        }
        else
        {
            rotationIndex = originalRotation;
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

    // --------------------------- Utilities ---------------------------

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

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
            return max - (min - input) % (max - min);
        else
            return min + (input - min) % (max - min);
    }
}