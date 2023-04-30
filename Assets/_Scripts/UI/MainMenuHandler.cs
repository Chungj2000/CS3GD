using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    
    private OptionsMenuHandler optionMenu;
    private LeaderboardUI leaderboardMenu;
    private const string newGame = "GameScene";
    private bool isLoadedGame = true;

    private void Start() {
        optionMenu = GetComponent<OptionsMenuHandler>();
        leaderboardMenu = GetComponent<LeaderboardUI>();
    }

    public void NewGameClicked() {
        //Debug.Log("New Game clicked.");
        SceneManager.LoadSceneAsync(newGame);
    }

    public void LoadClicked() {
        //Debug.Log("Load clicked.");

        //Create the scene using the save data so wave, score, and time is correct.
        PlayerPrefs.SetInt("loadGame", isLoadedGame ? 1 : 0);
        SceneManager.LoadSceneAsync(newGame);
    }

    public void LeaderboardClicked() {
        //Debug.Log("Leaderboard clicked.");
        leaderboardMenu.Show();
    }

    public void OptionsClicked() {
        //Debug.Log("Options clicked.");
        optionMenu.ShowOptions();
    }

    public void QuitClicked() {
        //Debug.Log("Quit clicked.");
        Application.Quit();
    }
}
