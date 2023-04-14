using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    
    private OptionsMenuHandler optionMenu;
    private const string newGame = "GameScene";

    private void Start() {
        optionMenu = GetComponent<OptionsMenuHandler>();
    }

    public void NewGameClicked() {
        Debug.Log("New Game clicked.");
        SceneManager.LoadSceneAsync(newGame);
    }

    public void LeaderboardClicked() {
        Debug.Log("Leaderboard clicked.");
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");
        optionMenu.ShowOptions();
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");
        Application.Quit();
    }
}
