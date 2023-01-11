using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInitialize : MonoBehaviour
{
    public Node[] PathNodes;
    void Awake() {
        // Debug.Log("HiHiHi");
        PathNodes = GetComponentsInChildren<Node>();
    }
}
