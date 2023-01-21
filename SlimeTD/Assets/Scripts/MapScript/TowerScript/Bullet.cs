using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifespan;
    private float bulletSpeed;
    private float bulletAtk;
    private float life;
    private Vector3 enemyPos;
    private Vector3 direction;
    
    void Start()
    {
        resetLife();

    }
    // Update is called once per frame
    void Update()
    {
        //==================

        life += Time.deltaTime;
        transform.position += direction * bulletSpeed * Time.deltaTime;
    }
    public void resetLife(){
        life = 0.0f;
    }
    public void setIniDirection(Vector3 dir){
        this.direction = dir;
    }

    public float getBulletAtk(){
        return bulletAtk;
    }
    public void setBulletAtk(float atk){
        this.bulletAtk = atk;
    }

    public float getBulletLifeSpan(){
        return this.lifespan;
    }
    public void setBulletLifeSpan(float lifespan){
        this.lifespan = lifespan;
    }
    public void setBulletSpeed(float speed){
        this.bulletSpeed = speed;
    }

    public float getLife(){
        return this.life;
    }
    float getDisSquared(Vector3 pos1,Vector3 pos2){
        return (((pos1.x - pos2.x) * (pos1.x - pos2.x)) + ((pos1.y - pos2.y) * (pos1.y - pos2.y)));
    }

    bool isColliAnyEnemy(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");
        float hitradiusSquared = 0.8f;
        foreach(GameObject go in enemies){
            float dis = getDisSquared(transform.position,go.transform.position);
            if(dis < hitradiusSquared){
                go.GetComponent<DefaultEnemyBehavior>().addHealth(-bulletAtk);
                Destroy(this.gameObject);
                if(go.GetComponent<DefaultEnemyBehavior>().getHealth() <= 0){
                    Destroy(go);
                    return true;
                }
            }
        }
        return false;
    }

}
