using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {
    
    private const string newGame = "GameScene";

    public void NewGameClicked() {
        SceneManager.LoadSceneAsync(newGame);
    }

    public void LoadClicked() {
        return;
    }

    public void SettingsClicked() {
        return;
    }

    public void QuitClicked() {
        Application.Quit();
    }
}
