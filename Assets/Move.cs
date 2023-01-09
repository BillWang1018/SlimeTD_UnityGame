using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerMove update
        Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W)) v.y += 0.1f;
        if (Input.GetKey(KeyCode.A)) v.x -= 0.1f;
        if (Input.GetKey(KeyCode.S)) v.y -= 0.1f;
        if (Input.GetKey(KeyCode.D)) v.x += 0.1f;

        transform.position += v;
        
    }
}
