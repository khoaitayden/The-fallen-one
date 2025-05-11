using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    private UIDocument StartMenuDocument;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Sprite easySprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hardSprite;
    //[SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject highscoreMenu;
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private AudioSource audioSource;
    private Button beginButton;
    private Button hightscoreButton;
    private Button backButton;
    private SliderInt hardSlider;
    private List<Button> menubuttons = new List<Button>();
    private Label hardTooltipLabel;
    private readonly string[] hardLabels = { "Easy", "Normal", "Hard" };

    public static int hardmode;
    private void Awake()
    {
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
        hardSlider = root.Q<SliderInt>("ChooseHard");
        hardTooltipLabel = root.Q<Label>("HardToolTip");


        beginButton.RegisterCallback<ClickEvent>(BeginGame);
        hightscoreButton.RegisterCallback<ClickEvent>(ShowHighScore);
        backButton.RegisterCallback<ClickEvent>(BackToMainMenu);
        hardSlider.RegisterValueChangedCallback(OnHardSliderChanged);

        hardSlider.RegisterCallback<MouseEnterEvent>(evt =>
        {
            hardTooltipLabel.style.display = DisplayStyle.Flex;
            UpdateTooltip(hardSlider.value);
        });
        hardSlider.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            hardTooltipLabel.style.display = DisplayStyle.None;
        });


        hardSlider.value = hardmode;
        UpdateHandleSprite(hardmode);

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
        GoToGameScene();
    }
    void GoToGameScene()
    {
        mainMenu.SetActive(false);
        gameObject.SetActive(false);
        sceneTransition.sceneTransition("level1");

    }
    void ShowHighScore(ClickEvent evt)
    {
        GoToHighScore();
    }
    void OnHardSliderChanged(ChangeEvent<int> evt)
    {
        hardmode = evt.newValue;
        Debug.Log("Hardness: " + hardmode);
        UpdateHandleSprite(hardmode);
        UpdateTooltip(hardmode);
    }
    void BackToMainMenu(ClickEvent evt)
    {
        GoToMainMenu();

    }
    void GoToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
    void GoToHighScore()
    {
        gameObject.SetActive(false);
        highscoreMenu.SetActive(true);
    }
    private void UpdateHandleSprite(int mode)
    {
        var handle = hardSlider.Q("unity-dragger");
        if (handle == null) return;

        switch (mode)
        {
            case 0:
                handle.style.backgroundImage = new StyleBackground(easySprite);
                break;
            case 1:
                handle.style.backgroundImage = new StyleBackground(normalSprite);
                break;
            case 2:
                handle.style.backgroundImage = new StyleBackground(hardSprite);
                break;
        }
    }
    private void UpdateTooltip(int mode)
    {
        if (hardTooltipLabel == null || hardSlider == null) return;

        var handle = hardSlider.Q("unity-dragger");
        if (handle == null) return;

        hardTooltipLabel.text = hardLabels[mode];

        // Position the tooltip near the handle using world-to-local
        var handleCenter = handle.worldBound.center;
        Vector2 localPos = hardTooltipLabel.parent.WorldToLocal(handleCenter);

        hardTooltipLabel.style.left = localPos.x-50;
        hardTooltipLabel.style.top= localPos.y+30; 
    }
}
