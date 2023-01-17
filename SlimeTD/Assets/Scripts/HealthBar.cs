using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float healthPercentage;
    private float maxHealth;
    private float currHealth;
    void Start()
    {
        maxHealth = GetComponentInParent<DefaultEnemyBehavior>().enemyData.health;
        currHealth = GetComponentInParent<DefaultEnemyBehavior>().getHealth();
    }
    void Update()
    {
        currHealth = GetComponentInParent<DefaultEnemyBehavior>().getHealth();
        healthPercentage = currHealth / maxHealth;
        GameObject.Find("Health").GetComponent<Image>().fillAmount = healthPercentage;
        GameObject.Find("Health").GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}
