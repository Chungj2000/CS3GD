using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour {
    
    public static ScoreTracker INSTANCE {get; private set;}
    private int currentScore;
    private TextMeshProUGUI scoreUI;

    private void Awake() {
        currentScore = 0;
        scoreUI = GetComponent<TextMeshProUGUI>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("InputManager instance created.");
        } else {
            //Debug.LogError("More than one InputManager instance created.");
            Destroy(gameObject);
            return;
        }
    
    }

    public int GetScore() {
        return currentScore;
    }

    public void AddScore(int addScoreValue) {
        if(currentScore + addScoreValue > currentScore) {
            currentScore += addScoreValue;
            //Debug.Log("Score Updated: " + currentScore);
            UpdateScoreText(currentScore);
        }
    }

    public void UpdateScoreText(int currentScore) {
        scoreUI.text = string.Format("Score: " + currentScore.ToString("00000000"));
    }

}
