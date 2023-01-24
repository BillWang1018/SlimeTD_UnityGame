using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int maxLife = 100;
    public int lifeCount;

    void Awake() {
        if(instance != null) {
            Debug.LogError("More than one BuildManager!");
            return;
        }
        instance = this;
        lifeCount = maxLife;
    }
    public void loseLife(int val) {
        lifeCount -= val;
        if(lifeCount <= 0) {
            lifeCount = 0;
            gameOver();
        }
    }
    void gameOver() {
        Debug.Log("RRrrrr you lost haha");
    }
    
}
