using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour {

    public static WaveSystem INSTANCE {get; private set;}

    [SerializeField] private int waveDelay = 5;
    [SerializeField] private Wave wave;
    
    private bool activeWave, nextWaveReady, waveDelayNotificationOn;
    private int waveCounter;
    private int enemiesInWave;
    private int notificationDuration = 3;

    private void Awake() {
        waveCounter = 1;
        enemiesInWave = 3;
        activeWave = false;
        nextWaveReady = false;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("WaveSystem instance created.");
        } else {
            //Debug.LogError("More than one WaveSystem instance created.");
            Destroy(gameObject);
            return;
        }
    
    }

    private void Update() {

        if(activeWave) {
            wave.Update();
        }

        if(!activeWave) {
            Debug.Log("New wave active.");
            activeWave = true;
            NewWave();
        }

        if(wave.IsWaveOver()) {

            //Debug.Log("Wave over.");

            StartCoroutine(WaveIntermission());

            if(nextWaveReady) {

                waveCounter += 1;
                StartCoroutine(NotificationHandler.INSTANCE.SetNotification("WAVE " + waveCounter, notificationDuration));
                //Debug.Log("Current wave: " + waveCounter);
                activeWave = false;
                nextWaveReady = false;

            }
        }

    }

    private void NewWave() {
        for(int i = 0; i < enemiesInWave * waveCounter; i++) {
            wave.SpawnEnemy();
        }
        EnemiesAliveTracker.INSTANCE.UpdateEnemyCount(GetEnemyCount());
    }

    IEnumerator WaveIntermission() {
        //Debug.Log("Next Wave In: " + waveDelay);
        yield return new WaitForSeconds(waveDelay);
        nextWaveReady = true;
    }

    public int GetEnemyCount() => wave.GetEnemyCount();

    [System.Serializable]
    private class Wave {

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Vector2 spawnRegion;
        private List<GameObject> enemies = new List<GameObject>();
        private GameObject enemyEntity;
        private bool waveOver;

        private void Start() {
            waveOver = false;
        }

        public void Update() {
            RemoveDeadEnemies();
            if(GetEnemyCount() <= 0) {
                waveOver = true;
                //Debug.Log("Wave now over.");
            }
        }

        public void SpawnEnemy() {

            waveOver = false;

            Vector3 randomSpawnLocation = new Vector3(
                UnityEngine.Random.Range(-spawnRegion.x, spawnRegion.x),
                0f,
                UnityEngine.Random.Range(-spawnRegion.y, spawnRegion.y)
            );

            enemyEntity = Instantiate(enemyPrefab, randomSpawnLocation, Quaternion.identity);
            enemies.Add(enemyEntity);

            Debug.Log("Enemy spawned count now at: " + GetEnemyCount());

        }
        
        public void RemoveDeadEnemies() {

            foreach(GameObject enemy in enemies.ToArray()) {

                if(enemy.GetComponent<EnemyAi>().IsDead()) {

                    enemies.Remove(enemy);
                    EnemiesAliveTracker.INSTANCE.UpdateEnemyCount(GetEnemyCount());
                    //Debug.Log("Enemy count reduced to: " + GetEnemyCount());

                }
            }

        }

        public int GetEnemyCount() {
            return enemies.Count;
        }

        public bool IsWaveOver() {
            return waveOver;
        }

    }
}
