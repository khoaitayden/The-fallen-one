using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void Move(float movespeed)
    {
        transform.position += Vector3.left * movespeed * Time.deltaTime;
    }
}
