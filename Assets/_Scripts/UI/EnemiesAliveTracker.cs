using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesAliveTracker : MonoBehaviour {

    public static EnemiesAliveTracker INSTANCE {get; private set;}
    private TextMeshProUGUI enemiesAliveUI;
    private int enemyCount;

    private void Awake() {
        enemiesAliveUI = GetComponent<TextMeshProUGUI>();
        enemyCount = 0;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("EnemiesAliveTracker instance created.");
        } else {
            //Debug.LogError("More than one EnemiesAliveTracker instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public void UpdateEnemyCount(int newEnemyCount) {
        if(enemyCount != newEnemyCount) {
            enemyCount = newEnemyCount;
            UpdateEnemyCountText();
        }
    }

    private void UpdateEnemyCountText() {
        enemiesAliveUI.text = string.Format("Enemies Alive: " + enemyCount.ToString("000"));
    }
}
