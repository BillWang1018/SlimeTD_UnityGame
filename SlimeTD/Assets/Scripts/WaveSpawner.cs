using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int waveIndex;
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public int maxWaveCount = 10;
    public float timeBetweenSpawn = 1f;
    public float timeBetweenWaves = 5f;
    private float countdownTimer;


    void Start() 
    {
        waveIndex = 0;
        countdownTimer = 0f;
    }
    void Update()
    {
        countdownTimer -= Time.deltaTime;
        if(countdownTimer <= 0) {
            if(++waveIndex <= maxWaveCount) {
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
    }
}
