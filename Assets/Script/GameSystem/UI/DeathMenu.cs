using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    
    private UIDocument deathMenuDocument;
    private Button againButton;
    private Button mainMenuButton;
    private AudioSource audioSource;
    
    private void Awake()
    {
        deathMenuDocument = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        var root = deathMenuDocument.rootVisualElement;
        
        againButton = root.Q<Button>("AgainButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");
        
        againButton.RegisterCallback<ClickEvent>(OnAgainClicked);
        mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuClicked);
    }
    
    private void OnDisable()
    {
        if (againButton != null)
            againButton.UnregisterCallback<ClickEvent>(OnAgainClicked);
            
        if (mainMenuButton != null)
            mainMenuButton.UnregisterCallback<ClickEvent>(OnMainMenuClicked);
    }
    
    private void OnAgainClicked(ClickEvent evt)
    {
        if (audioSource != null)
            audioSource.Play();
            
        StartCoroutine(DelayedAction(RestartGame));
    }
    
    private void OnMainMenuClicked(ClickEvent evt)
    {
        if (audioSource != null)
            audioSource.Play();
            
        StartCoroutine(DelayedAction(GoToMainMenu));
    }
    
    private IEnumerator DelayedAction(System.Action action)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        action();
    }
    
    private void RestartGame()
    {
        uiController.RestartGame();
    }
    
    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
            PlayerBehavior.isDead = false;
            PlayerController.canMove = true;
            
            if (StateManager.Instance != null)
            {
                StateManager.Instance.ResetScore();
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}