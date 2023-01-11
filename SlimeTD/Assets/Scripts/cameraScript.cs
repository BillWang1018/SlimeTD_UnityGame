using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
<<<<<<< HEAD
    // Start is called before the first frame update
    void Start()
    {
        
=======
    Vector3 mousePos;
    int changeAreaX;
    int changeAreaY;
    int scaler;
    float sensitive;
    // Start is called before the first frame update
    void Start()
    {
        sensitive = 0.05f;
        mousePos = Input.mousePosition;
        scaler = 30;
        changeAreaX = GetComponent<Camera>().pixelWidth / scaler;
        changeAreaY = GetComponent<Camera>().pixelHeight / scaler;
        Debug.Log("changeAreaX:" + changeAreaX);
        Debug.Log("changeAreaY:" + changeAreaY);
>>>>>>> 1453588807769f7c646590507a9e9e3d69c8b350
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        //Camera update
        if (Input.GetAxis("Mouse ScrollWheel") > 0) GetComponent<Camera>().orthographicSize -= 0.1f;

        if (Input.GetAxis("Mouse ScrollWheel") < 0) GetComponent<Camera>().orthographicSize += 0.1f;

=======
        //Camera Scale update
        if (Input.GetAxis("Mouse ScrollWheel") > 0) GetComponent<Camera>().orthographicSize -= 0.1f;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) GetComponent<Camera>().orthographicSize += 0.1f;

        //Camera move when mousePos near the boarder
        mousePos = Input.mousePosition;
        if (mousePos.x >= (GetComponent<Camera>().pixelWidth - changeAreaX) && (mousePos.x <= GetComponent<Camera>().pixelWidth)) GetComponent<Camera>().transform.position += new Vector3(sensitive, 0.0f,0.0f);
        if (mousePos.x <= changeAreaX && mousePos.x > 0) GetComponent<Camera>().transform.position -= new Vector3(sensitive, 0.0f, 0.0f);

        if (mousePos.y >= (GetComponent<Camera>().pixelHeight - changeAreaY) && (mousePos.y <= GetComponent<Camera>().pixelHeight)) GetComponent<Camera>().transform.position += new Vector3(0.0f, sensitive, 0.0f);
        if (mousePos.y <= changeAreaY && mousePos.y > 0) GetComponent<Camera>().transform.position -= new Vector3(0.0f, sensitive, 0.0f);
>>>>>>> 1453588807769f7c646590507a9e9e3d69c8b350
    }
}
