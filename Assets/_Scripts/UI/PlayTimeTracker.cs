using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayTimeTracker : MonoBehaviour {
    
    private TextMeshProUGUI playTimeUI;
    private float playTimer;

    private void Awake() {
        playTimeUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        playTimer = Time.timeSinceLevelLoad;
        int secs = (int) playTimer % 60;
        playTimer /= 60;
        int mins = (int) playTimer % 60;
        playTimer /= 60;
        int hours = (int) playTimer % 24;
        
        playTimeUI.text = string.Format("PLAY TIME | {0:D2}:{1:D2}:{2:D2}", hours, mins, secs);
    }
}
