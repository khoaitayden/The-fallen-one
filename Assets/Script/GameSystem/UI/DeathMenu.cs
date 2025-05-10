using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    
    private UIDocument deathMenuDocument;
    private Button againButton;
    private Button mainMenuButton;
    [SerializeField] private AudioSource audioSource;
    
    private void Awake()
    {
        deathMenuDocument = GetComponent<UIDocument>();
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
            
        Invoke("RestartGame",0.6f);
    }
    
    private void OnMainMenuClicked(ClickEvent evt)
    {
        if (audioSource != null)
            audioSource.Play();
        Invoke("GoToMainMenu",0.5f);
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