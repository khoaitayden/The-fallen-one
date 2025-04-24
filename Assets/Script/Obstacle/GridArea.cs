using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    public static bool isPlayerInsideTheGrid = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTheGrid = true;
            Debug.Log("Player entered area");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTheGrid = false;
            Debug.Log("Player left area");
        }
    }
}