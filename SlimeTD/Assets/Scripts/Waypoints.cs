using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    void Awake()
    {
        points = GetComponentsInChildren<Transform>();
    }
}
