using System;
using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerScore;

    public PlayerData(string name, int score)
    {
        playerName = name;
        playerScore = score;
    }
}

[System.Serializable]
public class GameLeaderboard
{
    public List<PlayerData> easyScores = new List<PlayerData>();
    public List<PlayerData> normalScores = new List<PlayerData>();
    public List<PlayerData> hardScores = new List<PlayerData>();
}