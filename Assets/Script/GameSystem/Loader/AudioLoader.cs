using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private const string MASTER_VOL = "MasterVolume";
    private const string SFX_VOL = "SFXVolume";
    private const string MUSIC_VOL = "MusicVolume";

    private void Start()
    {
        ApplySavedVolumes();
    }

    private void ApplySavedVolumes()
    {
        SetVolume(MASTER_VOL, PlayerPrefs.GetFloat(MASTER_VOL, 1f));
        SetVolume(SFX_VOL, PlayerPrefs.GetFloat(SFX_VOL, 1f));
        SetVolume(MUSIC_VOL, PlayerPrefs.GetFloat(MUSIC_VOL, 1f));
        Debug.Log("Loaded Master: " + PlayerPrefs.GetFloat(MASTER_VOL, -1f));
        Debug.Log("Loaded SFX: " + PlayerPrefs.GetFloat(SFX_VOL, -1f));
        Debug.Log("Loaded Music: " + PlayerPrefs.GetFloat(MUSIC_VOL, -1f));
    }

    private void SetVolume(string parameter, float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        dB = Mathf.Clamp(dB, -80f, 10f);
        audioMixer.SetFloat(parameter, dB);
    }
}