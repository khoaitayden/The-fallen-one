using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    // --------------------------- Public Properties ---------------------------

    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    [Header("Board Settings")]
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public TetrominoData[] tetrominoes;
    public GameObject piecePrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lineClearSFX;

    // --------------------------- Board Bounds ---------------------------

    public RectInt bounds
    {
        get
        {
            return new RectInt(-boardSize.x / 2, -boardSize.y / 2, boardSize.x, boardSize.y);
        }
    }

    // --------------------------- Unity Callbacks ---------------------------

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    // --------------------------- Piece Spawning ---------------------------

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        if (this.activePiece != null)
            Destroy(this.activePiece.gameObject);

        this.activePiece = Instantiate(piecePrefab, transform).GetComponent<Piece>();
        this.activePiece.Initialize(this, spawnPosition, data);
        Set(this.activePiece);
    }

    // --------------------------- Set & Clear Tiles ---------------------------

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int pos = piece.cells[i];
            if (bounds.Contains((Vector2Int)pos))
                tilemap.SetTile(pos, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int pos = piece.cells[i];
            if (bounds.Contains((Vector2Int)pos))
                tilemap.SetTile(pos, null);
        }
    }

    // --------------------------- Position Validation ---------------------------

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        return IsValidPosition(position, piece.cellOffsets);
    }

    public bool IsValidPosition(Vector3Int position, Vector3Int[] testOffsets)
    {
        RectInt bounds = this.bounds;

        for (int i = 0; i < testOffsets.Length; i++)
        {
            Vector3Int tilePosition = position + testOffsets[i];

            if (!bounds.Contains((Vector2Int)tilePosition) || tilemap.HasTile(tilePosition))
                return false;
        }

        return true;
    }

    // --------------------------- Line Clear Logic ---------------------------

    public void ClearLines()
    {
        StartCoroutine(ClearLinesCoroutine());
    }

    public IEnumerator ClearLinesCoroutine()
    {
        List<int> fullRows = new List<int>();

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            if (IsLineFull(y))
            {
                fullRows.Add(y);
            }
        }

        if (fullRows.Count > 0)
        {
            if (audioSource && lineClearSFX)
            {
                audioSource.PlayOneShot(lineClearSFX);
            }
            yield return StartCoroutine(AnimateLineClear(fullRows));

            foreach (int y in fullRows)
            {
                ClearLine(y);
                ShiftLinesDown(y);
            }
        }
    }

    private bool IsLineFull(int y)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            if (!tilemap.HasTile(pos))
                return false;
        }
        return true;
    }

    private void ClearLine(int y)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            tilemap.SetTile(pos, null);
        }
    }

    private void ShiftLinesDown(int fromY)
    {
        for (int y = fromY + 1; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int from = new Vector3Int(x, y, 0);
                Vector3Int to = new Vector3Int(x, y - 1, 0);

                TileBase tile = tilemap.GetTile(from);
                tilemap.SetTile(to, tile);
                tilemap.SetTile(from, null);
            }
        }
    }

    // --------------------------- Line Clear Animation ---------------------------

    private IEnumerator AnimateLineClear(List<int> rows)
    {
        float duration = 0.5f;
        int blinkCount = 3;

        for (int i = 0; i < blinkCount; i++)
        {
            SetLineColor(rows, i % 2 == 0 ? Color.white : Color.clear);
            yield return new WaitForSeconds(duration / (blinkCount * 2));
        }

        float fadeTime = 0.3f;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            float t = elapsed / fadeTime;
            Color fadeColor = Color.Lerp(Color.white, Color.clear, t);
            SetLineColor(rows, fadeColor);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetLineColor(rows, Color.white); // reset color
    }

    private void SetLineColor(List<int> rows, Color color)
    {
        foreach (int y in rows)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTileFlags(pos, TileFlags.None);
                tilemap.SetColor(pos, color);
            }
        }
    }
}