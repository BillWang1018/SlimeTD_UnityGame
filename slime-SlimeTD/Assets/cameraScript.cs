using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Camera update
        if (Input.GetAxis("Mouse ScrollWheel") > 0) GetComponent<Camera>().orthographicSize -= 0.1f;

        if (Input.GetAxis("Mouse ScrollWheel") < 0) GetComponent<Camera>().orthographicSize += 0.1f;

    }
}
