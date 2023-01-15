using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager2 : MonoBehaviour
{
    [SerializeField]
    private Tilemap towerMap;
    [SerializeField]
    private List<TowerData> towerDatas;
    private Dictionary<TileBase, TowerData> tilebaseToData; 
    
    void Awake() 
    {

        tilebaseToData = new Dictionary<TileBase, TowerData>();

        foreach(var towerData in towerDatas) {
            foreach(var tower in towerData.tiles) {
                tilebaseToData.Add(tower, towerData);
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            TileBase tb = towerMap.GetTile(MousePosition.tilePos);
            if(tb != null) {
                Debug.Log(tilebaseToData[tb]);

            }
        }
    }
}
