using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour {
    
    private List<int> leaderboardScores = new List<int>();

    private void Start() {
        LeaderboardSystem.INSTANCE.GetHighScores();
    }

    public void AddScore(int score) {
        leaderboardScores.Add(score);
        //Debug.Log(score + " added.");
        SortLeaderboard();
    }

    //Sort in descending order.
    private void SortLeaderboard() {
        leaderboardScores.Sort();
        leaderboardScores.Reverse();
        
    
        foreach(int score in leaderboardScores) {
            Debug.Log(score);
        }
    
    }

    public List<int> GetLeaderboardScores() {
        return leaderboardScores;
    }

}
