using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration=1f ;
    [SerializeField] private Animator fadeAnimator;
    public bool FadedIn=false;
    public void sceneTransition(string sceneName)
    {
        FadeIn();
        StartCoroutine(FadeOutSound(musicSource, fadeDuration));
        StartCoroutine(LoadScene(sceneName));
    }
    public void FadeIn()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeIn");
        }
        FadedIn=true;

    }
    public IEnumerator FadeOutSound(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
    }
    public IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneName);
            yield return null;
    }

}
