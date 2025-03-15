using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MiddleOb1ScoreCounter : MonoBehaviour
{
    void Start()
    { 
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            {
                ScoreData.Instance.AddScoreToOb1(1);
                Debug.Log("Current Score: ");
            }
        }
    }
}
