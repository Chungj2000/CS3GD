using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayTimeTracker : MonoBehaviour {

    public static PlayTimeTracker INSTANCE {get; private set;}

    private TextMeshProUGUI playTimeUI;
    private float playTimer;

    private void Awake() {
        playTimer = 0f;
        playTimeUI = GetComponent<TextMeshProUGUI>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayTimeTracker instance created.");
        } else {
            //Debug.LogError("More than one PlayTimeTracker instance created.");
            Destroy(this);
            return;
        }
    }

    private void Update() {

        //Update the timer.
        playTimer += Time.deltaTime;

        //Format the timer as 00:00:00.
        string hours = Mathf.Floor(playTimer / 3600).ToString("00");
        string mins = Mathf.Floor(playTimer / 60).ToString("00");
        string secs = Mathf.RoundToInt(playTimer % 60).ToString("00");
        
        playTimeUI.text = string.Format("PLAY TIME | {0}:{1}:{2}", hours, mins, secs);
    }

    public float GetPlayTime() {
        return playTimer;
    }

    public void SetPlayTime(float playTime) {
        playTimer = playTime;
        Debug.Log("Play time set to: " + playTimer);
    }
}
