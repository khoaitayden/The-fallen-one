using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private InputAction pauseAction;
    [SerializeField] private InputAction restartAction;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform zoomTarget;
    [SerializeField] private Animator animator;
    private bool isPaused = false;
    

    void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += OnPausePerformed;

        restartAction.Enable();
        restartAction.performed += OnRestartPerformed;

        PlayerBehavior.OnPlayerDied += DeathMenu;
    }

    void OnDisable()
    {
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();

        restartAction.performed -= OnRestartPerformed;
        restartAction.Disable();

        PlayerBehavior.OnPlayerDied -= DeathMenu;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Pause action performed");
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    private void OnRestartPerformed(InputAction.CallbackContext context)
    {
        if (deathMenu.activeSelf) // Only allow restart if death screen is shown
        {
            RestartGame();
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Obstacle.SpeedMultiplier = 1f;
        PlayerBehavior.isDead = false;
        PlayerController.canMove = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void DeathMenu()
    {
        if (deathMenu.activeSelf) return;
        StartCoroutine(DeathSequence());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private IEnumerator DeathSequence()
    {
        // Slow down time
        Time.timeScale = 0.2f;

        float duration = 1f;
        float elapsed = 0f;

        // Store original camera position
        Vector3 startPos = mainCamera.transform.position;

        // Set target position to zoom toward (keep same z value)
        Vector3 targetPos = new Vector3(zoomTarget.position.x, zoomTarget.position.y-1.5f, startPos.z);

        // Store original zoom level
        float startSize = mainCamera.orthographicSize;

        // Set target zoom level
        float endSize = 1.5f;
        animator.SetTrigger("death");
        // Interpolate position and zoom over time
        while (elapsed < duration)
        {
            // Use unscaled time so this still works during slow motion
            elapsed += Time.unscaledDeltaTime;

            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, endSize, elapsed / duration);

            yield return null;

        }
        
        // Ensure final position and zoom are correct
        mainCamera.transform.position = targetPos;
        mainCamera.orthographicSize = endSize;

        // Wait in real time
        yield return new WaitForSecondsRealtime(1f);

        // Pause the game completely
        Time.timeScale = 0f;

        // Show the death menu
        deathMenu.SetActive(true);
        Time.timeScale = 1f;
    }

}