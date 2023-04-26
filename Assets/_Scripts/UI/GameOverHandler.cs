using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverHandler : MonoBehaviour {

    public static GameOverHandler INSTANCE {get; private set;}
    private const string retry = "GameScene";
    private const string mainMenu = "MainMenu";

    [SerializeField] private Canvas gameOverOverlayUI;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI yourScoreText;
    private int highScore, yourScore;
    private bool checkedLeaderboardScore = false;

    private void Awake() {

        HideGameOver();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GameOverHandler instance created.");
        } else {
            //Debug.LogError("More than one GameOverHandler instance created.");
            Destroy(gameObject);
            return;
        }

    }
    
    public void RetryClicked() {
        //Debug.Log("Retry clicked.");
        SceneManager.LoadSceneAsync(retry);
    }

    public void MainMenuClicked() {
        //Debug.Log("Main Menu clicked.");
        SceneManager.LoadSceneAsync(mainMenu);
    }

    private void ShowGameOver() {
        gameOverOverlayUI.enabled = true;
    }

    private void HideGameOver() {
        gameOverOverlayUI.enabled = false;
    }

    public void LoadGameOver() {

        SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetGameOverSFX(), SoundSystem.INSTANCE.GetAudioSourceSFX());
        
        INSTANCE.ShowGameOver();
        Debug.Log("Game Over Screen loaded.");

        yourScore = ScoreTracker.INSTANCE.GetScore();

        //LeaderboardSystem.INSTANCE.SetHighScore(ScoreTracker.INSTANCE.GetScore());

        //Update UI visual text for Scores.
        WriteHighScore();

        CheckLeaderboardScore();

        if(CheckNewHighScore()) {
            WriteNewHighScore();
        } else {
            WriteYourScore();
        }
        
    }

    private void WriteHighScore() {
        highScoreText.text = string.Format("HIGH SCORE: " + highScore.ToString("00000000"));
    }

    private bool CheckNewHighScore() {
        if(highScore < yourScore) {
            return true;
        }
        return false;
    }

    //Check if the score should be added to the leaderboard.
    private void CheckLeaderboardScore() {

        if(checkedLeaderboardScore) {
            return;
        }

        List<int> leaderboardScores = LeaderboardSystem.INSTANCE.GetLeaderboard().GetLeaderboardScores();

        for(int i = 0; i < leaderboardScores.Count; i++) {

            if(leaderboardScores[i] < yourScore) {

                Debug.Log("Index at: " + i);
                checkedLeaderboardScore = true;
                LeaderboardSystem.INSTANCE.SetNewScoreIndex(i);
                LeaderboardSystem.INSTANCE.SetHighScore(yourScore);
                break;

            } else {
                Debug.Log("New index: " + i);
            }

        }
    }
    

    private void WriteNewHighScore() {
        yourScoreText.text = string.Format("NEW HIGH SCORE: " + yourScore.ToString("00000000"));
    }

    private void WriteYourScore() {
        yourScoreText.text = string.Format("YOUR SCORE: " + yourScore.ToString("00000000"));
    }

    public void SetHighScore(int score) {
        highScore = score;
    }

}