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
            gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerBehavior.TriggerPlayerDied();
        Debug.Log("Player Died");
    }
    
}
