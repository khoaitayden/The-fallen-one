using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize;

    public RectInt bounds
    {
        get
        {
            return new RectInt(-boardSize.x / 2, -boardSize.y / 2, boardSize.x, boardSize.y);
        }
    }
    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        this.activePiece.Initialize(this, spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            tilemap.SetTile(piece.cells[i], piece.data.tile); // Already world position
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            tilemap.SetTile(piece.cells[i], null); // Already world position
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = position + (Vector3Int)piece.data.cells[i];

            // Check bounds
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // Check collision with *other* tiles
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }
}
