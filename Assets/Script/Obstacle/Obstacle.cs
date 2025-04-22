using UnityEngine;
using UnityEngine.Rendering;
using System;
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
        if (transform.position.x < -20f) 
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
                PlayerBehavior.TriggerPlayerDied();
        }
        
    }
    
}
