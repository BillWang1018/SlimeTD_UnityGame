using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdate : MonoBehaviour
{
    public TMP_Text lifeCounter;
    public TMP_Text waveCounter; 
    public TMP_Text waveCountdownTimer;

    void Update() {
        lifeCounter.text = "Life: " + GameManager.lifeCount.ToString();

        waveCounter.text = "Wave: " + WaveSpawner.waveIndex.ToString();

        if(WaveSpawner.countdownTimer < WaveSpawner.timeBetweenWaves) {
            waveCountdownTimer.text = "Wave incoming in " + Mathf.Round(WaveSpawner.countdownTimer).ToString();
        } else {
            waveCountdownTimer.text = "";
        }
    }


}
