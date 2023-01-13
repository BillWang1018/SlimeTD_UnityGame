using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // How fast do you want it to yeeeeet
    public float MovingSpeed;
    //health
    public float Health;
    // Nodes of path
    private static Node[] PathNodes;
    // Which node are we
    private int NodeIndex = 0;
    // To count how many time passed from node to node (for Lerp())
    private float Timer;
    // Fun Vector3 that store Node positions
    private Vector3 PreviousPosition;
    private Vector3 NextPosition;
    private GameObject Path;
    
    // Start is called before the first frame update
    void Start()
    {
        Path = GameObject.FindGameObjectWithTag("Path");
        PathNodes = Path.GetComponent<PathInitialize>().PathNodes;
        NodeIndex = 0;
        Timer = 0;
        PreviousPosition = transform.position;
        // Debug.Log(PathNodes.Length);
        NextPosition = PathNodes[0].transform.position;
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
            // move object in a linear motion
            if(transform.position != NextPosition) {
                transform.position = Vector3.Lerp(PreviousPosition, NextPosition, Timer);
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

    void OnCollisionEnter2D(Collision2D e){
        //=================Slime modify here=================
        Health -= Bullet.bulletDmg;
        if(Health <= 0){
            Destroy(this.gameObject);
        }
        Destroy(e.gameObject,0.0f);
    }

}
