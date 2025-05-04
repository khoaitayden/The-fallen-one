using UnityEngine;
using System.Collections.Generic;

public class Creature4PoolingManager : MonoBehaviour
{
    [SerializeField]private GameObject creature4Prefab;
    [SerializeField]private int initialCount = 10;
    [SerializeField]private float spawnRadius = 5f;
    [SerializeField]private Transform target;
    [SerializeField]private float creatureSpeed;

    private List<Creature4> activeCreatures = new List<Creature4>();
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = Instantiate(creature4Prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        SpawnCreatures(initialCount);
    }

    public void SpawnCreatures(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (pool.Count == 0) return;

            GameObject creatureObj = pool.Dequeue();
            creatureObj.SetActive(true);
            Vector3 randomPos = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
            creatureObj.transform.position = randomPos;

            Creature4 creature = creatureObj.GetComponent<Creature4>();
            creature.speed = creatureSpeed;
            activeCreatures.Add(creature);
        }

        foreach (var creature in activeCreatures)
        {
            creature.Init(target, activeCreatures);
        }
    }

    public void DespawnCreature(Creature4 creature)
    {
        if (activeCreatures.Contains(creature))
        {
            activeCreatures.Remove(creature);
            creature.gameObject.SetActive(false);
            pool.Enqueue(creature.gameObject);
        }
    }
}