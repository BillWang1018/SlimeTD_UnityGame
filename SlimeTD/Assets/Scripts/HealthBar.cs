using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    private float healthPercentage;
    private float maxHealth;
    private float currHealth;
    void Start()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
        maxHealth = GetComponentInParent<DefaultEnemyBehavior>().enemyData.health;
        currHealth = GetComponentInParent<DefaultEnemyBehavior>().getHealth();
    }
    void Update()
    {
        currHealth = GetComponentInParent<DefaultEnemyBehavior>().getHealth();
        healthPercentage = currHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}
