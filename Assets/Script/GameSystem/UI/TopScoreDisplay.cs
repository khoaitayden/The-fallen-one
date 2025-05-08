using UnityEngine;
using UnityEngine.UI;
public class ScoreEntry : MonoBehaviour 
{
    public Text playerNameText;
    public Text scoreText;

    public void SetEntry(string name, int score)
    {
        playerNameText.text = name;
        scoreText.text = score.ToString();
    }
}