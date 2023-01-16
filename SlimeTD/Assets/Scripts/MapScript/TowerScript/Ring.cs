using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private float ringScaleSpeed;
    private float ringDamage;
    private float ringSpeed;
    private float life;
    private float lifespan;
    private float curnRadius;
    private float hitradiusSquared = 0.8f;
    //the enemies Health have been decreased 
    List<GameObject> enemyEncountered;
    LineRenderer circleRenderer;
    float radius;
    void Start()
    {
        enemyEncountered = new List<GameObject>();
        life = 0.0f;
        curnRadius = 0.0f;
        radius = 8.0f;
        print("radius : "+ radius);
        gameObject.transform.localScale = new Vector3(0.0f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //get the circle 2 * radius
       
        life += Time.deltaTime;
        if(life > lifespan){//destroy obj after amount of time
            Destroy(this.gameObject);
        }

        
        curnRadius += radius * ringScaleSpeed * Time.deltaTime;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");

        gameObject.transform.localScale = new Vector3(curnRadius,curnRadius);
        foreach(GameObject g in enemies){
            // curn pos -> enemy pos
            Vector3 v = g.transform.position - transform.position;
            Vector3 pos = v.normalized * curnRadius;
            if(!enemyEncountered.Contains(g) && getDisSquared(g.transform.position , pos) <= hitradiusSquared){
                //do damage
                g.GetComponent<PathFollower>().Health -= ringDamage;

                //add enemy to the hurted list
                enemyEncountered.Add(g);
            }
        }
    }

    float getDisSquared(Vector3 pos1,Vector3 pos2){
        return (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y);
    }

    public void setRingAtk(float atk){
        this.ringDamage = atk;
    }
    public void setRingAtkSped(float atkSped){
        this.ringSpeed = atkSped;
    }
    public void setRingLifeSpan(float lifespan){
        this.lifespan = lifespan;
    }
    public void setRingScaleSpeed(float ringScaleSpeed){
        this.ringScaleSpeed = ringScaleSpeed;
    }
}
