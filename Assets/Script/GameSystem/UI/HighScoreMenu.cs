using UnityEngine;
using System.Collections.Generic;
public class HighScoreMenu : MonoBehaviour
{
    // Assign these in the Inspector
    [SerializeField] private ScoreEntry[] easyScoreEntries; 
    [SerializeField] private ScoreEntry[] normalScoreEntries;
    [SerializeField] private ScoreEntry[] hardScoreEntries;
    [SerializeField] private GameObject easyScoreBoard; 
    [SerializeField] private GameObject normalScoreBoard;
    [SerializeField] private GameObject hardScoreBoard;
    [SerializeField] private GameObject startMenu;
    [SerializeField] AudioSource buttonSound; 

    void Start()
    {
        DisplayScores(SaveManager.GetScoresByDifficulty(0), easyScoreEntries);
        DisplayScores(SaveManager.GetScoresByDifficulty(1), normalScoreEntries);
        DisplayScores(SaveManager.GetScoresByDifficulty(2), hardScoreEntries);
        ChooseBoardToDisplay(StartMenu.hardmode);
    }
    void OnEnable()
    {
        DisplayScores(SaveManager.GetScoresByDifficulty(0), easyScoreEntries);
        DisplayScores(SaveManager.GetScoresByDifficulty(1), normalScoreEntries);
        DisplayScores(SaveManager.GetScoresByDifficulty(2), hardScoreEntries);
        ChooseBoardToDisplay(StartMenu.hardmode);
    }
    void ChooseBoardToDisplay(int hardmode)
    {
        switch (hardmode)
        {
            case 0:
                easyScoreBoard.SetActive(true);
                normalScoreBoard.SetActive(false);
                hardScoreBoard.SetActive(false);
                break;
            case 1:
                easyScoreBoard.SetActive(false);
                normalScoreBoard.SetActive(true);
                hardScoreBoard.SetActive(false);
                break;
            case 2:
                easyScoreBoard.SetActive(false);
                normalScoreBoard.SetActive(false);
                hardScoreBoard.SetActive(true);
                break;
        }
    }
    void DisplayScores(List<PlayerData> scores, ScoreEntry[] entries)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            if (i < scores.Count)
            {
                entries[i].SetEntry(scores[i].playerName, scores[i].playerScore);
            }
            else
            {
                entries[i].SetEntry("-", 0);
            }
        }
    }
    public void BackToMainMenu()
    {
        buttonSound.Play();
        Invoke(nameof(GoToMainMenu), 0.5f);
        
    }
    void GoToMainMenu()
    {
        
        gameObject.SetActive(false);
        startMenu.SetActive(true);
    }
}