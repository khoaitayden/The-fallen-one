using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "leaderboard.json");

    public static void SavePlayerData(string playerName, int score, int difficulty)
    {
        GameLeaderboard leaderboard = LoadLeaderboard();
        PlayerData newEntry = new PlayerData(playerName, score);
        switch (difficulty)
        {
            case 0:
                leaderboard.easyScores.Add(newEntry);
                leaderboard.easyScores.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));
                if (leaderboard.easyScores.Count > 6) leaderboard.easyScores.RemoveRange(6, leaderboard.easyScores.Count - 6);
                break;
            case 1:
                leaderboard.normalScores.Add(newEntry);
                leaderboard.normalScores.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));
                if (leaderboard.normalScores.Count > 6) leaderboard.normalScores.RemoveRange(6, leaderboard.normalScores.Count - 6);
                break;
            case 2:
                leaderboard.hardScores.Add(newEntry);
                leaderboard.hardScores.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));
                if (leaderboard.hardScores.Count > 6) leaderboard.hardScores.RemoveRange(6, leaderboard.hardScores.Count - 6);
                break;
            default:
                Debug.LogError("Invalid difficulty: " + difficulty);
                return;
        }
        string jsonData = JsonUtility.ToJson(leaderboard, true);
        File.WriteAllText(SavePath, jsonData);
    }

    public static GameLeaderboard LoadLeaderboard()
    {
        if (File.Exists(SavePath))
        {
            string jsonData = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameLeaderboard>(jsonData);
        }
        return new GameLeaderboard();
    }
}