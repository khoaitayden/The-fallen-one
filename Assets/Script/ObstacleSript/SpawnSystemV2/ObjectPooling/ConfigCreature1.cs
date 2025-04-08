using UnityEngine;

public class ConfigCreature1 : ConfigObstacle
{
    public  float maxSpacing;
    public float minSpacing;   
    public float minHeight;
    public float maxHeight;
    [SerializeField] CreateCreature1 createCreature1;
    public Obstacle Creature1;
    public override void WrappReuse()
    {
        if (createCreature1 == null || createCreature1.pooledObjects == null || createCreature1.pooledObjects.Count == 0)
        return;
        Reuse(createCreature1.pooledObjects, createCreature1.amountToPool);
    }
    public override Vector3 GenerateRandomPosition(int i)
    {
        float randomy = Random.Range(minHeight, maxHeight);
        float randomx = Random.Range(minSpacing, maxSpacing);
        Vector3 randomPosition = new Vector3(randomx, randomy, 0);
        return randomPosition;
    }


}
