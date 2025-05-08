using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.UI;
#endif
public class EndLv2 : MonoBehaviour
{
    [Header("Camera Shake Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.3f;
    [SerializeField] private GameObject creature5;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Rigidbody2D creature5rb;
    [SerializeField] private UIController uiController;
    [SerializeField] private TMP_InputField inputField;
    private bool freeze = true; 
    private string playerName;
    private Vector3 originalPos;

    private void OnEnable()
    {
        StateMangagerLv2.OnCountdownFinished += HandleCountdownFinished;
    }

    private void OnDisable()
    {
        StateMangagerLv2.OnCountdownFinished -= HandleCountdownFinished;
    }

    private void HandleCountdownFinished()
    {
        Debug.Log("Countdown finished! Reacting in another script.");
        creature5.SetActive(true);
        Creature5LauchUp();
        LockPlayerMovement(freeze);
        StartCoroutine(ShakeCamera());
    }
    private void Creature5LauchUp()
    {
        if (creature5 != null && creature5rb != null)
        {
            Vector2 launchDirection = new Vector2(0f, 1f); 
            float launchForce = 9f; 
            creature5rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
    }
    private void LockPlayerMovement(bool freeze)
{
    if (player != null)
    {
        if (freeze)
        {
            player.linearVelocity = Vector2.zero; 
            player.angularVelocity = 0f;
            player.constraints = RigidbodyConstraints2D.FreezeAll;
        } 
    }
}
    private IEnumerator ShakeCamera()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not assigned!");
            yield break;
        }

        originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
        SaveScoreandGoToMainMenu();
        
    }
    private void SaveScoreandGoToMainMenu()
    {
        endMenu.SetActive(true);
        StartCoroutine(InputName());
    }
    IEnumerator InputName()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        } 
        playerName=inputField.text;
        SaveManager.SavePlayerData(playerName,StateMangagerLv2.Instance.Score, StartMenu.hardmode);
        uiController.LoadMainMenu();
    }
    
}