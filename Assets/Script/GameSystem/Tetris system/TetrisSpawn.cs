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
        GameObject newPiece=Instantiate(tetrominoPrefabs[index], spawnPosition, Quaternion.identity);
        activePiece = newPiece.GetComponent<TetrisBlock>();
    }
}