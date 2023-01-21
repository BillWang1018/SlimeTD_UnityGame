using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifespan;
    private float bulletSpeed;
    private float bulletAtk;
    private float life;
    Vector3 enemyPos;
    Vector3 velocity;
    void Start()
    {
        life = 0.0f;
        enemyPos = getNearestEnemyPos(transform.position);
        Vector2 direction = enemyPos - transform.position;
       
        //rb = GetComponent<Rigidbody2D>();
        velocity = direction.normalized * bulletSpeed;
        
        float rotz = Mathf.Atan2(-direction.y,-direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotz);

    }
    // Update is called once per frame
    void Update()
    {
        //==================

        life += Time.deltaTime;
        if(life > lifespan){
            Destroy(this.gameObject,0);
        }
        transform.position += velocity * Time.deltaTime;
        isColliAnyEnemy();
    }
    Vector3 getNearestEnemyPos(Vector3 pos){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");
        if(enemies.Length == 0)return new Vector3(0.0f,0.0f,0.0f);

        Vector3 resPos = new Vector3(0.0f,0.0f,0.0f);
        float minDis = float.MaxValue;
        float dis = 0.0f;
        foreach(GameObject g in enemies){
            dis = getDisSquared(g.transform.position,pos);
            if(dis < minDis){
                minDis = dis;
                resPos = g.transform.position;
            }
        }
        return resPos;
    }

    public float getBulletAtk(){
        return bulletAtk;
    }
    public void setBulletAtk(float atk){
        this.bulletAtk = atk;
    }
    public void setBulletLifeSpan(float lifespan){
        this.lifespan = lifespan;
    }
    public void setBulletSpeed(float speed){
        this.bulletSpeed = speed;

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
