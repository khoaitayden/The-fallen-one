using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class CreateCreature1 : CreateObstacle
{
    public override bool CheckCanSpawn()
    {
        if (ScoreManager.Instance.Score>=50) return true;
        else return false;
    }
    public override Vector3 GenerateRandomPosition(int i)
    {
        float randomy= Random.Range(-3.8f, 3.8f);
        float randomx= Random.Range(15f, 50f);
        Vector3 randomPosition = new Vector3(randomx, randomy, 0);
        return randomPosition;
    }
}
