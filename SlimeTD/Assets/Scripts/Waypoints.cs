using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void DrawLine() {
        for(int i=0; i < points.Length; i++) {
            if(i < points.Length-1) {
                Debug.DrawLine(points[i].position, points[i+1].position, Color.yellow);
            }
        }
    }
    void Awake()
    {
        // sussy lambda function to exclude parent itself
        points = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
        Debug.Log(points.Length);
    }
    void Update() {
        DrawLine();
    }
}
