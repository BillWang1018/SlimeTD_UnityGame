using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public TileBase towerA;
    void Start() {
        buildManager = BuildManager.instance;
    }
    public void BuyStandardTurret()
    {
        if(buildManager.selectedBuilding != towerA) {
            buildManager.selectedBuilding = towerA;
            Debug.Log("Turret Selected.");
        } else {
            buildManager.selectedBuilding = null;
            Debug.Log("Cancel Selection");
        }
    }
}
