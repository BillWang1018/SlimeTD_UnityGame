using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int waveIndex;
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public int maxWaveCount = 10;
    public float timeBetweenSpawn = 1f;
    public float __TimeBetweenWaves__ = 5.5f;
    public static float timeBetweenWaves;
    public static float countdownTimer;
    private bool isSpawning;

    void Awake()
    {
        timeBetweenWaves = __TimeBetweenWaves__;
    }
    void Start() 
    {
        waveIndex = 0;
        countdownTimer = timeBetweenWaves;
        isSpawning = false;
    }
    void Update()
    {
        if(!isSpawning)
            countdownTimer -= Time.deltaTime;
        if(countdownTimer <= 0) {
            isSpawning = true;
            if(waveIndex < maxWaveCount) {
                waveIndex++;
                Debug.Log(">>> Spawning wave "+waveIndex);
                StartCoroutine(SpawnWave());
                countdownTimer = timeBetweenWaves;
            }
        }

    }

    void SpawnEnemy() {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    IEnumerator SpawnWave() 
    {
        for(int i=0; i < waveIndex; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        isSpawning = false;
    }
}
