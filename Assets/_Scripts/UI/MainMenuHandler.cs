using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    
    private const string newGame = "GameScene";

    public void NewGameClicked() {
        Debug.Log("New Game clicked.");
        SceneManager.LoadSceneAsync(newGame);
    }

    public void LeaderboardClicked() {
        Debug.Log("Leaderboard clicked.");
    }

    public void SettingsClicked() {
        Debug.Log("Settings clicked.");
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");
        Application.Quit();
    }
}
