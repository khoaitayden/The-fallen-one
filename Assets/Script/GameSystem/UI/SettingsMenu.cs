using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class SettingsMenu : MonoBehaviour
{
    private UIDocument settingsMenuDocument;
    private Button backButton;
    private AudioSource audioSource;
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu; 
    
    private bool isInGame = false; 
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        settingsMenuDocument = GetComponent<UIDocument>();
    }
    
    private void OnEnable()
    {
        var root = settingsMenuDocument.rootVisualElement;
        backButton = root.Q<Button>("BackButton");
        backButton.RegisterCallback<ClickEvent>(HandleBackButton);
    }
    
    void OnDisable()
    {
        backButton.UnregisterCallback<ClickEvent>(HandleBackButton);
    }
    
    public void OpenFromMainMenu()
    {
        isInGame = false;
        gameObject.SetActive(true);
    }
    
    public void OpenFromGame()
    {
        isInGame = true;
        gameObject.SetActive(true);
    }
    
    private void HandleBackButton(ClickEvent evt)
    {
        if (audioSource != null)
            audioSource.Play();
        
        StartCoroutine(DelayedGoBack());
    }
    
    private IEnumerator DelayedGoBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GoBack();
    }
    
    private void GoBack()
    {
        gameObject.SetActive(false);
        
        if (isInGame)
        {
            if (pauseMenu != null)
                pauseMenu.SetActive(true);
            else
                ResumeGame();
        }
        else
        {
            mainMenu.SetActive(true);
        }
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}