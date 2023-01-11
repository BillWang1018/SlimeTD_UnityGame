using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed=5f;
    private Transform previousWaypoint;
    private Transform nextWaypoint;
    private int waypointIndex;
    private float time, timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        waypointIndex = 0;
        previousWaypoint = Waypoints.points[0];
        if(Waypoints.points.Length > 1) {
            nextWaypoint = Waypoints.points[1];
        } else {
            Destroy(gameObject);
        }
        time = Vector3.Distance(previousWaypoint.position, nextWaypoint.position)/speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(transform.position != nextWaypoint.position) {
            transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, timer/time);
        } else {
            timer = 0;
            if(waypointIndex+2 < Waypoints.points.Length) {
                waypointIndex++;
                previousWaypoint = Waypoints.points[waypointIndex];
                nextWaypoint = Waypoints.points[waypointIndex+1];
                time = Vector3.Distance(previousWaypoint.position, nextWaypoint.position)/speed;
            } else {
                Destroy(gameObject);
            }
        }

    }
}
