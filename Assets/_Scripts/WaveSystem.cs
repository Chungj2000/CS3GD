using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour {

    public static WaveSystem INSTANCE {get; private set;}

    [SerializeField] private int waveDelay = 5;
    [SerializeField] private Wave wave;
    
    private bool activeWave, nextWaveReady, waveDelayNotificationOn, isCouroutineStarted;
    private int waveCounter;
    private int enemiesInWave;
    private int notificationDuration = 3;
    private static string currentWaveNotification, waveIntermissionNotification;

    private void Awake() {

        waveIntermissionNotification = "NEXT WAVE IN " + waveDelay + " SECONDS";

        waveCounter = 1;
        enemiesInWave = 3;

        activeWave = false;
        nextWaveReady = false;
        isCouroutineStarted = false;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("WaveSystem instance created.");
        } else {
            //Debug.LogError("More than one WaveSystem instance created.");
            Destroy(this);
            return;
        }
    
    }

    private void Update() {

        if(activeWave) {
            wave.Update();
        }

        if(!activeWave) {
            //Debug.Log("New wave active.");
            activeWave = true;
            NewWave();
        }

        if(wave.IsWaveOver()) {

            //Debug.Log("Wave over.");

            //Ensure couroutine is only ran once.
            if(!isCouroutineStarted) {
                StartCoroutine(WaveIntermission());
            }

            //Prepare logic for the next wave once wave intermission elapses.
            if(nextWaveReady) {

                waveCounter += 1;
                currentWaveNotification = "WAVE " + waveCounter;

                //Notify current wave.
                StartCoroutine(NotificationHandler.INSTANCE.SetTimedWaveNotification(currentWaveNotification, notificationDuration));
                //Debug.Log("Current wave: " + waveCounter);
                
                activeWave = false;
                nextWaveReady = false;
                isCouroutineStarted = false;

            }
        }

    }

    private void NewWave() {

        //Spawned enemies is scaled based on enemies in a wave by the current wave.
        for(int i = 0; i < enemiesInWave * waveCounter; i++) {
            wave.SpawnEnemy();
        }

        //Update ScreenSpace UI after all enemies have been spawned.
        EnemiesAliveTracker.INSTANCE.UpdateEnemyCount(GetEnemyCount());
    }

    IEnumerator WaveIntermission() {
        //Debug.Log("Next wave in " + waveDelay + " seconds.");

        //This couroutine is currently running, therefore disable other calls to this function.
        isCouroutineStarted = true;

        //Notify when next wave will occur.
        StartCoroutine(NotificationHandler.INSTANCE.SetTimedWaveNotification(waveIntermissionNotification, notificationDuration));

        yield return new WaitForSeconds(waveDelay);
        nextWaveReady = true;
    }

    public int GetEnemyCount() => wave.GetEnemyCount();

    public int GetWaveCount() {
        return waveCounter;
    }

    public void SetWaveCount(int waveCount) {
        waveCounter = waveCount;
        Debug.Log("Wave set to: " + waveCounter);
    }

    [System.Serializable]
    private class Wave {

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Vector2 spawnRegion;
        [SerializeField] private LayerMask outOfBounds;
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

            Vector3 randomSpawnLocation;

            waveOver = false;

            //Prevent enemies from spawning inside terrain prefabs.
            do {

                //Generate a random valid position on the map.
                randomSpawnLocation = new Vector3(
                    UnityEngine.Random.Range(-spawnRegion.x, spawnRegion.x),
                    0f,
                    UnityEngine.Random.Range(-spawnRegion.y, spawnRegion.y)
                );
            

            } while(Physics2D.OverlapCircle(new Vector3(randomSpawnLocation.x, randomSpawnLocation.y, randomSpawnLocation.z), 1f, outOfBounds));

            //Create the enemy at the random position and add it to the list of enemies.
            enemyEntity = Instantiate(enemyPrefab, randomSpawnLocation, Quaternion.identity);
            enemies.Add(enemyEntity);

            //Debug.Log("Enemy spawn count now at: " + GetEnemyCount());

        }
        
        public void RemoveDeadEnemies() {

            //Cycle through the list of enemies, and remove any that are 'Dead'
            foreach(GameObject enemy in enemies.ToArray()) {

                if(enemy.GetComponent<EnemyAi>().IsDead()) {

                    enemies.Remove(enemy);
                    //Update ScreenSpace UI after an enemy has been removed.
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
