using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardUI : MonoBehaviour {
    
    [SerializeField] private Canvas leaderboardCanvas;
    [SerializeField] private TextMeshProUGUI firstPlace;
    [SerializeField] private TextMeshProUGUI secondPlace;
    [SerializeField] private TextMeshProUGUI thirdPlace;
    [SerializeField] private TextMeshProUGUI fourthPlace;
    [SerializeField] private TextMeshProUGUI fifthPlace;

    private TextMeshProUGUI[] leaderboardRanks;

    private void Start() {
        Hide();

        //List rankings in descending order.
        leaderboardRanks = new TextMeshProUGUI[]{firstPlace,
                                                 secondPlace, 
                                                 thirdPlace, 
                                                 fourthPlace, 
                                                 fifthPlace};
    }
    
    public void PopulateLeaderboard(List<int> leaderboardScores) {
        for(int x = 0; x < leaderboardRanks.Length; x++) {
            leaderboardRanks[x].text = string.Format(leaderboardScores[x].ToString("00000000"));
        }
        Debug.Log("Leaderboard populated.");
    }

    public void CloseClicked() {
        Hide();
    }

    public void Show() {
        leaderboardCanvas.enabled = true;
    }

    public void Hide() {
        leaderboardCanvas.enabled = false;
    }

}
