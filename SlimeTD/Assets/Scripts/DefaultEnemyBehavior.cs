using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyBehavior : MonoBehaviour
{
    public EnemyData enemyData;
    private float speed;
    private float health, maxHealth;
    private Transform previousWaypoint;
    private Transform nextWaypoint;
    private int waypointIndex;
    private float time, timer;

    //special effects
    private float poisonPerSec, buffedPercentage, nerfedPercentage, regeneratePerSec;

    void Awake() 
    {
        //get set-up value from enemyData
        gameObject.GetComponent<SpriteRenderer>().sprite = enemyData.enemyImage;
        speed = enemyData.speed;
        health = maxHealth = enemyData.health;
        buffedPercentage = enemyData.buffedPercentage;
        nerfedPercentage = enemyData.nerfedPercentage;
        regeneratePerSec = enemyData.regeneratePerSec;

    }
    void Start()
    {
        //==[[test]]==//
        setRegenerate(10);
        //==[[----]]==//
        timer = 0;
        waypointIndex = 0;
        previousWaypoint = Waypoints.points[0];
        if(Waypoints.points.Length > 1) {
            nextWaypoint = Waypoints.points[1];
        } else {
            ReachEnd();
        }
        time = Vector3.Distance(previousWaypoint.position, nextWaypoint.position)/speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(transform.position != nextWaypoint.position) {
            transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, timer/time);
        } else {
            timer = 0;
            if(waypointIndex+2 < Waypoints.points.Length) {
                waypointIndex++;
                previousWaypoint = Waypoints.points[waypointIndex];
                nextWaypoint = Waypoints.points[waypointIndex+1];
                time = Vector3.Distance(previousWaypoint.position, nextWaypoint.position)/speed;
            } else {
                ReachEnd();
            }
        }

        // update special effects
        tickPoisoned(Time.deltaTime);
        tickRegenerate(Time.deltaTime);

    }

    void ReachEnd() {
        GameManager.instance.loseLife(1);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D e){
        if(e.gameObject.tag == "Bullet")
            health -= GetComponent<Bullet>().getBulletAtk(); 
        if(health <= 0){
            Destroy(this.gameObject);
        }
        Destroy(e.gameObject,0.0f);
    }

    void tickPoisoned(float amp=1) { // amp -> amplitude
        health -= poisonPerSec*amp;
    }
    void tickRegenerate(float amp=1)  {
        health += regeneratePerSec*amp;
        if(health >= maxHealth) health = maxHealth;
    }


    //====== get / set ======//
    public float getHealth() {
        return this.health;
    }
    public void setHealth(float health) {
        this.health = health;
    }
    public void attacked(float h) {
        this.health -= h * (1+nerfedPercentage);
    }
    public float getSpeed() {
        return this.speed;
    }
    public void setSpeed(float speed) {
        this.speed = speed;
    }
    public float getPoisoned() {
        return poisonPerSec;
    }
    public void setPoisoned(float val) {
        this.poisonPerSec = val;
    }
    public float getRegenerate() {
        return regeneratePerSec;
    }
    public void setRegenerate(float val) {
        this.regeneratePerSec = val;
    }
    public float getBuffed() {
        return buffedPercentage;
    }
    public void setBuffed(float val) {
        this.buffedPercentage = val;
    }
    public float getNerfed() {  
        return nerfedPercentage;
    }
    public void setNerfed(float val) {
        this.nerfedPercentage = val;
    }
}
