using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int waveIndex;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int maxWaveCount = 10;
    public float timeBetweenSpawn = 1f;
    public float timeBetweenWaves = 5.5f;
    public float countdownTimer;
    private bool isSpawning;
    [SerializeField]
    private List<EnemyData> enemyDatas;

    void Awake() 
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

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint) {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        // v this is for fun v
        newEnemy.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0.1f, 0.5f, 0.75f, 1f, 0.8f, 1f);
    }
    IEnumerator SpawnWave() 
    {
        for(int i=0; i < waveIndex; i++) {
            SpawnEnemy(enemyPrefab, spawnPoint);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        isSpawning = false;
    }
}
