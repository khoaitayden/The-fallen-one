using UnityEngine;

public class ObstacleCreature1 : Obstacle
{
    [SerializeField] private float speed;
    public float Creature1Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    void Update()
    {
        Move(speed);
    }
}
