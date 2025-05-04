using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using System.Collections;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    private const string MASTER_VOL = "MasterVolume";
    private const string SFX_VOL = "SFXVolume";
    private const string MUSIC_VOL = "MusicVolume";

    private UIDocument settingsMenuDocument;
    private Button backButton;
    private Slider masterSlider, sfxSlider, musicSlider;
    private AudioSource audioSource;

    [Header("Menus")]
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

        masterSlider = root.Q<VisualElement>("MasterVolume")?.Q<Slider>();
        sfxSlider = root.Q<VisualElement>("SFXVolume")?.Q<Slider>();
        musicSlider = root.Q<VisualElement>("MusicVolume")?.Q<Slider>();

        masterSlider.RegisterValueChangedCallback(evt => SetVolume(MASTER_VOL, evt.newValue));
        sfxSlider.RegisterValueChangedCallback(evt => SetVolume(SFX_VOL, evt.newValue));
        musicSlider.RegisterValueChangedCallback(evt => SetVolume(MUSIC_VOL, evt.newValue));

        LoadVolumeSettings();
    }

    private void OnDisable()
    {
        backButton.UnregisterCallback<ClickEvent>(HandleBackButton);
    }

    private void SetVolume(string parameter, float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        dB = Mathf.Clamp(dB, -80f, 10f);
        audioMixer.SetFloat(parameter, dB);
        PlayerPrefs.SetFloat(parameter, value);
    }

    private void LoadVolumeSettings()
    {
        float master = PlayerPrefs.GetFloat(MASTER_VOL, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_VOL, 1f);
        float music = PlayerPrefs.GetFloat(MUSIC_VOL, 1f);

        masterSlider.value = master;
        sfxSlider.value = sfx;
        musicSlider.value = music;

        SetVolume(MASTER_VOL, master);
        SetVolume(SFX_VOL, sfx);
        SetVolume(MUSIC_VOL, music);
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