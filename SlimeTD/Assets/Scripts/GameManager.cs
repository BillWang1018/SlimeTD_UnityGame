using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxLife = 100;
    public static int lifeCount;

    void Awake() {
        lifeCount = maxLife;
    }
    
}
