using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    private UIDocument StartMenuDocument;
    [SerializeField] private GameObject mainMenu;
    //[SerializeField] private GameObject settingsMenu;
    private Button beginButton;
    private Button hightscoreButton;
    private Button backButton;
    private Slider hardSlider;
    private List<Button> menubuttons = new List<Button>();
    private AudioSource audioSource;

    public static int hardmode=0;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        StartMenuDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = StartMenuDocument.rootVisualElement;

        menubuttons = root.Query<Button>().ToList();
        for (int i = 0; i < menubuttons.Count; i++)
        {
            menubuttons[i].RegisterCallback<ClickEvent>(OnAllButtonClicked);
        }

        beginButton = root.Q<Button>("BeginButton");
        hightscoreButton = root.Q<Button>("HighScoreButton");
        backButton = root.Q<Button>("BackButton");
        hardSlider = root.Q<Label>("ChooseHard").Q<Slider>();

        beginButton.RegisterCallback<ClickEvent>(BeginGame);
        hightscoreButton.RegisterCallback<ClickEvent>(ShowHighScore);
        backButton.RegisterCallback<ClickEvent>(BackToMainMenu);
        hardSlider.RegisterValueChangedCallback(OnHardSliderChanged);
    }

    private void OnDisable()
    {
        for (int i = 0; i < menubuttons.Count; i++)
        {
            menubuttons[i].UnregisterCallback<ClickEvent>(OnAllButtonClicked);
        }

        beginButton.UnregisterCallback<ClickEvent>(BeginGame);
        hightscoreButton.UnregisterCallback<ClickEvent>(ShowHighScore);
        backButton.UnregisterCallback<ClickEvent>(BackToMainMenu);
        hardSlider.UnregisterValueChangedCallback(OnHardSliderChanged);
    }
    private void OnAllButtonClicked(ClickEvent evt)
    {
        audioSource.Play();
    }
    void BeginGame(ClickEvent evt)
    {
        hardmode=0;
        Invoke(nameof(GoToGameScene), 0.5f);
    }
    void GoToGameScene()
    {
        mainMenu.SetActive(false);
        gameObject.SetActive(false);
        SceneManager.LoadScene("level1");
    }
    void ShowHighScore(ClickEvent evt)
    {
        //Invoke(nameof(GoToHighScore), 0.5f);
    }
    void OnHardSliderChanged(ChangeEvent<float> evt)
    {
        hardmode = Mathf.RoundToInt(evt.newValue);
        Debug.Log("Hardness: " + hardmode);
    }
    void BackToMainMenu(ClickEvent evt)
    {
        Invoke(nameof(GoToMainMenu), 0.5f);

    }
    void GoToMainMenu()
    {
        //settingsMenu.SetActive(false);
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
