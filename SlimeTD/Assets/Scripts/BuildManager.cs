using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public TileBase selectedBuilding;

    // Terrain = ore, and Tower = Facility+Tower (conbined for now)
    public Tilemap Terrain, Tower;
    public TileBase road;
    void Awake() 
    {
        if(instance != null) {
            Debug.LogError("More than one BuildManager!");
            return;
        }
        instance = this;
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            // Debug.Log("Mouse down");
            buildTower();
        }
    }

    public void buildTower() {
        if(checkValid()) {
            Tower.SetTile(MousePosition.tilePos, selectedBuilding);
            selectedBuilding = null;
        } else {
            return;
        }
    }
    public bool checkValid() {
        if(Terrain.GetTile(MousePosition.tilePos) != null && 
            Terrain.GetTile(MousePosition.tilePos) != road && 
            Tower.GetTile(MousePosition.tilePos) == null) 
        {
            return true;
        }
        // Debug.Log("Bruh you cannot place here");
        return false;
    }


}
