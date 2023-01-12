using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyBehavior : MonoBehaviour
{
    public float speed=5f;
    public static int health=10;
    private Transform previousWaypoint;
    private Transform nextWaypoint;
    private int waypointIndex;
    private float time, timer;

    void Start()
    {
        timer = 0;
        waypointIndex = 0;
        previousWaypoint = Waypoints.points[0];
        if(Waypoints.points.Length > 1) {
            nextWaypoint = Waypoints.points[1];
        } else {
            ReachEnd();
        }
        time = Vector3.Distance(previousWaypoint.position, nextWaypoint.position)/speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(health <= 0) {
            Destroy(gameObject);
        }
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
                ReachEnd();
            }
        }

    }

    void ReachEnd() {
        GameManager.lifeCount--;
        Debug.Log(GameManager.lifeCount);
        Destroy(gameObject);
    }
}
