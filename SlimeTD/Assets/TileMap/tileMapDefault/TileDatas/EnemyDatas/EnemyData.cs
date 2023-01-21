using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "Custom/Enemy Comfiguration")]
public class EnemyData : ScriptableObject
{
    public Sprite enemyImage;
    public float speed, health;
}



