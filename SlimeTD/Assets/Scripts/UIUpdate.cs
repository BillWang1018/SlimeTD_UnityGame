using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdate : MonoBehaviour
{
    public TMP_Text lifeCounter;
    public TMP_Text waveCounter; 
    public TMP_Text waveCountdownTimer;
    private float countdownTimer, timeBetweenWaves;

    void Update() {
        lifeCounter.text = "Life: " + GameManager.lifeCount.ToString();

        waveCounter.text = "Wave: " + WaveSpawner.waveIndex.ToString();

        countdownTimer = GameObject.Find("Wave Manager").GetComponent<WaveSpawner>().countdownTimer;
        timeBetweenWaves = GameObject.Find("Wave Manager").GetComponent<WaveSpawner>().timeBetweenWaves;
        if(countdownTimer < timeBetweenWaves) {
            waveCountdownTimer.text = "Wave incoming in " + Mathf.Round(countdownTimer).ToString();
        } else {
            waveCountdownTimer.text = "";
        }
    }


}
