using UnityEngine;

public class Obstacle1 : Obstacle
{
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        Move(speed);
    }
    public float Obstacle1Speed
    {
        get { return speed; }
        set { speed = value; }
    }
}
