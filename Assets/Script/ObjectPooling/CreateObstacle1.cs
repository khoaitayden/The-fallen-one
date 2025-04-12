using UnityEngine;
using System.Collections.Generic;
public class CreateObstacle1 : CreateObstacle
{
    public override bool CheckCanSpawn()
        {
            return true;
        }
    public override Vector3 GenerateRandomPosition(int i)
    {
        float randomy= Random.Range(-4.4f, 1.3f);
        Vector3 randomPosition = new Vector3(11+(i*9), randomy, 0);
        return randomPosition;
        
    }
}
