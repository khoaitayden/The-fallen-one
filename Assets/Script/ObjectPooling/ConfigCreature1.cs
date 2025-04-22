using UnityEngine;

public class ConfigCreature1 : ConfigObstacle
{
    public  float maxSpacing;
    public float minSpacing;   
    public float minHeight;
    public float maxHeight;
    public static bool canreuse=true;
    [SerializeField] CreateCreature1 createCreature1;
    public Obstacle Creature1;
    public override void WrappReuse()
    {
        if (createCreature1 == null || createCreature1.pooledObjects == null || createCreature1.pooledObjects.Count == 0) return;
        if (canreuse==false) Destroyall();
        Reuse(createCreature1.pooledObjects, createCreature1.amountToPool);
    }
    public override Vector3 GenerateRandomPosition(int i)
    {
        float randomy = Random.Range(minHeight, maxHeight);
        float randomx = Random.Range(minSpacing, maxSpacing);
        Vector3 randomPosition = new Vector3(randomx, randomy, 0);
        return randomPosition;
    }
    private void OnEnable()
    {
        PlayerBehavior.OnPlayerDied += ResetReuse;
    }

    private void OnDisable()
    {
        PlayerBehavior.OnPlayerDied -= ResetReuse;
    }
    private void ResetReuse()
    {
        canreuse = true;
    }
    public void Destroyall()
    {
        for (int i = 0; i < createCreature1.pooledObjects.Count; i++)
        {
            Destroy(createCreature1.pooledObjects[i]);
        }
        createCreature1.pooledObjects.Clear();
    }
}
