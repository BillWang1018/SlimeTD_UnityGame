using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // Nodes of path
    Node[] PathNodes;
    // It's.. yeah... the object that... want to moves
    GameObject[] MovingObjects;
    // How fast do you want it to yeeeeet
    public float MovingSpeed;
    // Which node are we
    private int NodeIndex;
    // To count how many time passed from node to node (for Lerp())
    private float Timer;
    // Fun Vector3 that store Node positions
    private Vector3 PreviousPosition;
    private Vector3 NextPosition;

    
    // Start is called before the first frame update
<<<<<<< HEAD
    void Start()
=======
    void Start()//er just close it?
>>>>>>> 1453588807769f7c646590507a9e9e3d69c8b350
    {
        // get cute moving obj
        MovingObjects = GameObject.FindGameObjectsWithTag("MovingDude");
        // get cute Nodes
        PathNodes = GetComponentsInChildren<Node>();

        NodeIndex = 0;
        Timer = 0;
            PreviousPosition = MovingObjects[0].transform.position;
            NextPosition = PathNodes[NodeIndex].transform.position;
    }

    void DrawLine() {
        for(int i=0; i < PathNodes.Length; i++) {
            if(i < PathNodes.Length-1) {
                Debug.DrawLine(PathNodes[i].transform.position, PathNodes[i+1].transform.position, Color.yellow);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();
        // move when there are still nodes
        if(NodeIndex < PathNodes.Length) {
            //Debug.Log(NodeIndex);
            Timer += Time.deltaTime * MovingSpeed;

            foreach(GameObject MovingObject in MovingObjects) {
                // move object in a linear motion
                if(MovingObject.transform.position != NextPosition) {
                    MovingObject.transform.position = Vector3.Lerp(PreviousPosition, NextPosition, Timer);
                } else { // when it reachs next node
                    Timer = 0;
                    if(NodeIndex+1 < PathNodes.Length) {
                        Timer = 0;
                        PreviousPosition = PathNodes[NodeIndex].transform.position;
                        NextPosition = PathNodes[++NodeIndex].transform.position;
                    }
                }
            }
        }
    }
}
