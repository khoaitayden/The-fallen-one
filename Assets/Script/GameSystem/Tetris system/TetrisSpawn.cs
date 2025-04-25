using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    [Header("Tetrimino Settings")]
    public GameObject[] tetrominoPrefabs; // Drag your Tetris piece prefabs here
    public TetrisBlock activePiece;

    [Header("Spawn Settings")]
    public Vector3 spawnPosition; // Adjust as needed

    private void Start()
    {
        SpawnNextPiece();
    }

    public void SpawnNextPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        GameObject newPiece = Instantiate(tetrominoPrefabs[index], spawnPosition, Quaternion.identity);
        activePiece = newPiece.GetComponent<TetrisBlock>();

        if (IsOverlappingGrid(activePiece))
        {
            PlayerBehavior.TriggerPlayerDied();
        }
    }
    private bool IsOverlappingGrid(TetrisBlock piece)
    {
        foreach (Transform child in piece.transform)
        {
            Vector2 pos = child.position;
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);

            if (x >= 0 && x < TetrisBlock.wide && y >= 0 && y < TetrisBlock.height)
            {
                if (TetrisBlock.grid[x, y] != null)
                    return true;
            }
        }
        return false;
    }
}