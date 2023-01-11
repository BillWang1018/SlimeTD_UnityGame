using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifespan;
    private float life;
    private Camera cam;
    Rigidbody2D rb;
    Vector3 mousePos;
    void Start()
    {
        life = 0.0f;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
       
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized;
        
        float rotz = Mathf.Atan2(-direction.y,-direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotz);

    }

    // Update is called once per frame
    void Update()
    {
        life += Time.deltaTime;
        if(life > lifespan){
            Destroy(this.gameObject,0);
        }
    }
}
