using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MiddlePart : MonoBehaviour
{
    void Start()
    { 
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            {
                StateManager.Instance.IncreaseHardAfterPassedPipe(1);
                Debug.Log("Current Score: ");
            }
        }
    }
}
