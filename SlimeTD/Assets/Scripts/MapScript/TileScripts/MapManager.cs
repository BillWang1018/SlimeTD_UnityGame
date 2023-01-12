using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour{

    [SerializeField]
    private Tilemap map;
    
    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase,TileData> dataFromTiles;

    private void Awake(){
        dataFromTiles = new Dictionary<TileBase,TileData>();

        foreach(TileData tileData in tileDatas){

            foreach(Tile tile in tileData.tiles){
                dataFromTiles.Add(tile,tileData);

            }
        }
    }

    private void Update(){
        if(Input.GetMouseButton(0)){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = map.WorldToCell(mousePos);
            TileBase clickedTile = map.GetTile(gridPos);
            TileBase clickedTileUp = map.GetTile(gridPos + new Vector3Int(0,1));
            float oreLevel = dataFromTiles[clickedTile].oreLevel;
            float oreLevelUp = dataFromTiles[clickedTileUp].oreLevel;

            print("At pos:" + gridPos + " there's a " + clickedTile + " has oreLevel: " + oreLevel + " oreLevelUp:" + oreLevelUp);

        }
    }
}
