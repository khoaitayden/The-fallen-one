using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "leaderboard.json");
    public static List<PlayerData> GetEasyScores()
    {
        return LoadLeaderboard().easyScores;
    }

    public static List<PlayerData> GetNormalScores()
    {
        return LoadLeaderboard().normalScores;
    }

    public static List<PlayerData> GetHardScores()
    {
        return LoadLeaderboard().hardScores;
    }

    public static List<PlayerData> GetScoresByDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0: 
                return GetEasyScores();
            case 1:
                return GetNormalScores();
            case 2: 
                return GetHardScores();
            default:
                return new List<PlayerData>();
        }
    }
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