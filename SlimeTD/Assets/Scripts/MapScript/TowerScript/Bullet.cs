using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float __lifespan__ = 10f;
    public float __BulletSpeed__ = 5f;
    public float __BulletDmg__ = 2f;
    public static float lifespan, bulletSpeed, bulletDmg;
    private float life;
    Rigidbody2D rb;
    Vector3 enemyPos;
    void Awake()
    {
        lifespan = __lifespan__; bulletSpeed = __BulletSpeed__; bulletDmg = __BulletDmg__;
    }
    void Start()
    {
        life = 0.0f;
        enemyPos = getNearestEnemyPos(transform.position);
        Vector2 direction = enemyPos - transform.position;
       
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * bulletSpeed;
        
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

    float getDisSquared(Vector3 pos1,Vector3 pos2){
        return (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z);
    }

    

}
