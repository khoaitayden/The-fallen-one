using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Piece : MonoBehaviour
{
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Board board { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex{ get; private set; }
    
    public void Initialize(Board board,Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = position + (Vector3Int)data.cells[i];
        }
    }
private void Update()
{
    this.board.Clear(this);
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        Rotation(-1);
    } else if (Input.GetKeyDown(KeyCode.LeftControl))
    {
        Rotation(1);
    }
    if (Input.GetKeyDown(KeyCode.A))
    {
        Move(Vector2Int.left);
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
        Move(Vector2Int.right);
    }
    if (Input.GetKeyDown(KeyCode.S))
    {
        Move(Vector2Int.down);
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
        HardDrop();
    }
    this.board.Set(this);
}
    private void HardDrop()
    {
        while (Move(Vector2Int.down)) {
            continue;
        }
    }
    private bool Move(Vector2Int direction)
{
    Vector3Int newPosition = this.position + new Vector3Int(direction.x, direction.y, 0);
    bool valid = board.IsValidPosition(this, newPosition);

    if (valid)
    {
        this.position = newPosition;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = this.position + (Vector3Int)data.cells[i];
        }
    }

    return valid;
}
    private void Rotation(int direction)
{
    this.rotationIndex += WrapRotation(this.rotationIndex + direction,0,4);
    for (int i=0;i<this.cells.Length;i++)
    {
        Vector3 cell = this.cells[i];
        int x,y;
        switch (this.data.tetromino)
        {
            case Tetromino.I:
            case Tetromino.O:
                cell.x-=0.5f;
                cell.y-=0.5f;
                x=Mathf.CeilToInt((cell.x*Data.RotationMatrix[0]*direction)+(cell.y*Data.RotationMatrix[1]*direction));
                y=Mathf.CeilToInt((cell.x*Data.RotationMatrix[2]*direction)+(cell.y*Data.RotationMatrix[3]*direction));
                break;
            default:
                x=Mathf.RoundToInt((cell.x*Data.RotationMatrix[0]*direction)+(cell.y*Data.RotationMatrix[1]*direction));
                y=Mathf.RoundToInt((cell.x*Data.RotationMatrix[2]*direction)+(cell.y*Data.RotationMatrix[3]*direction));
                break;
        }
        this.cells[i]=new Vector3Int(x,y,0);
    }
}
    private int WrapRotation(int input, int min, int max)
    {
        if (input<min)
        {
            return max-(min-input)%(max-min);
        } else 
        {
            return min+(input-min)%(max-min);
        }
    }
}