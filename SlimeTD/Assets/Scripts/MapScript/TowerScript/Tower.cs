using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Tower : MonoBehaviour
{
    /*
    public float atkSpeed;
    public float atkDamage;
    public float bulletLifeSpan;
    public GameObject bullet;
    Vector3 enemyPos;
    float time;
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        enemyPos = getNearestEnemyPos(transform.position);
        time += Time.deltaTime;
        if(enemyPos == new Vector3(0.0f,0.0f,0.0f))return;
        
        Vector2 v = enemyPos - transform.position;
        float rotz = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotz);
        
        
        if(time >= atkSpeed){
            time = 0.0f;
            GameObject g = Instantiate(bullet,transform.position,Quaternion.identity);
            g.GetComponent<Bullet>().setBulletAtk(atkDamage);
            g.GetComponent<Bullet>().setBulletLifeSpan(bulletLifeSpan);
            
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
    */
}
