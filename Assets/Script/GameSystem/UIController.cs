using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private InputAction pauseAction;
    [SerializeField] private InputAction restartAction;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void DeathMenu()
    {
        deathMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}