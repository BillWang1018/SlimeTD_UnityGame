using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float atkSpeed;
    public float atkDamage;
    
    public GameObject bullet;
    private Camera cam;
    Vector3 mousePos;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 v = mousePos - transform.position;
        float rotz = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotz);

        Instantiate(bullet,transform.position,Quaternion.identity);
    }
}
