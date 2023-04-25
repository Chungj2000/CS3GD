using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour {
    
    public static PauseMenuHandler INSTANCE {get; private set;}

    private OptionsMenuHandler optionMenu;
    private LeaderboardUI leaderboardUI;
    private const string mainMenu = "MainMenu";
    private bool isPauseActive;

    [SerializeField] private Canvas pauseOverlayUI;

    private void Awake() {

        isPauseActive = false;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PauseMenuHandler instance created.");
        } else {
            //Debug.LogError("More than one PauseMenuHandler instance created.");
            Destroy(gameObject);
            return;
        }

    }

    private void Start() {
        optionMenu = GetComponent<OptionsMenuHandler>();
        leaderboardUI = LeaderboardSystem.INSTANCE.GetLeaderboardUI();
    }

    private void Update() {
        PauseMenuDisplay();
    }
    
    public void ResumeClicked() {
        Debug.Log("Resume clicked.");
        ToggleIsPauseActive();
    }

    public void LeaderboardClicked() {
        Debug.Log("Load clicked.");
        leaderboardUI.Show();
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");
        optionMenu.ShowOptions();
    }

    public void MainMenuClicked() {
        Debug.Log("Main Menu clicked.");

        //If currently paused, resume timeScale so session resumes normally before main menu transition.
        if(isPauseActive) {
            ToggleIsPauseActive();
        }

        SceneManager.LoadSceneAsync(mainMenu);
    }

    private void ToggleIsPauseActive() {
        //Switch between bool states when pause is triggered.
        isPauseActive = !isPauseActive;
        //Change the timeScale based on the paused state.
        PauseTimeScale();
    }

    private void PauseMenuDisplay() {
        pauseOverlayUI.enabled = isPauseActive;
    }

    public void TogglePauseMenu() {
        
        INSTANCE.ToggleIsPauseActive();
        Debug.Log("Pause Screen toggled to: " + isPauseActive);
        
    }

    //Manipulate the time scale depending on whether pause is active or not.
    private void PauseTimeScale() {
        if(isPauseActive) {
            //Stop all timed instances when paused.
            Time.timeScale = 0;
        } else {
            //Resume timed instances when pause state is over.
            Time.timeScale = 1;
        }
    }

    public bool IsPauseActive() {
        return isPauseActive;
    }

}
