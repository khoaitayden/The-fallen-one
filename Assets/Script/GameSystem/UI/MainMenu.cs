using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    private UIDocument MainMenuDocument;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject settingsMenu;
    private Button startButton;
    private Button SettingsButton;
    private Button exitButton;
    private List<Button> menubuttons = new List<Button>();
    private AudioSource audioSource;

private void Awake()
{
    audioSource = GetComponent<AudioSource>();
    MainMenuDocument = GetComponent<UIDocument>();
}
    private void OnEnable()
    {
        var root = MainMenuDocument.rootVisualElement;

        menubuttons = root.Query<Button>().ToList();
        for (int i = 0; i < menubuttons.Count; i++)
        {
            menubuttons[i].RegisterCallback<ClickEvent>(OnAllButtonClicked);
        }

        startButton = root.Q<Button>("StartButton");
        SettingsButton = root.Q<Button>("SettingsButton");
        exitButton = root.Q<Button>("QuitButton");

        startButton.RegisterCallback<ClickEvent>(StartGame);
        SettingsButton.RegisterCallback<ClickEvent>(OpenSettings);
        exitButton.RegisterCallback<ClickEvent>(ExitGame);
    }

    private void OnDisable()
    {
        for (int i = 0; i < menubuttons.Count; i++)
        {
            menubuttons[i].UnregisterCallback<ClickEvent>(OnAllButtonClicked);
        }

        startButton.UnregisterCallback<ClickEvent>(StartGame);
        SettingsButton.UnregisterCallback<ClickEvent>(OpenSettings);
        exitButton.UnregisterCallback<ClickEvent>(ExitGame);
    }

    private void StartGame(ClickEvent evt)
    {
        Invoke(nameof(GoToStartMenu), 0.5f);
    }

    private void OpenSettings(ClickEvent evt)
    {
        Invoke(nameof(GoToSettingsMenu), 0.5f);
    }

    private void GoToStartMenu()
    {
        startMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void GoToSettingsMenu()
    {
        settingsMenu.SetActive(true);
        var settingsMenuScript = settingsMenu.GetComponent<SettingsMenu>();
        if (settingsMenuScript != null)
        {
            settingsMenuScript.OpenFromMainMenu();
        }
        gameObject.SetActive(false);
    }

    private void ExitGame(ClickEvent evt)
    {
        Debug.Log("Exit Game button clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnAllButtonClicked(ClickEvent evt)
    {
        audioSource.Play();
    }
}