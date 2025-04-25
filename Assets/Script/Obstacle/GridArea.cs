using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    public static bool isPlayerInsideTheGrid = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTheGrid = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTheGrid = false;
        }
    }
}