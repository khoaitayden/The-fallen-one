using UnityEngine;
using UnityEngine.Rendering;

public class Obstacle : MonoBehaviour
{
    public float movespeed;
    public static float SpeedMultiplier = 1f;
    public float CurrentSpeed;
    void Start()
    {
        CurrentSpeed = movespeed * SpeedMultiplier;
    }
    void Update()
    {
        CurrentSpeed = movespeed*SpeedMultiplier;
        transform.position += Vector3.left * (movespeed*SpeedMultiplier) * Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit");
    }
}
