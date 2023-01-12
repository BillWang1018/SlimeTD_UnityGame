using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour{
    //=====================Ore=====================
    [SerializeField]
    private Tilemap OreTileMap;
    
    [SerializeField]
    private List<OreData> OreDatas;

     //=====================Facility=====================
    [SerializeField]
    private Tilemap FacilityTileMap;
    
    [SerializeField]
    private List<FacilityData> FacilityDatas;
    //=====================Tower=====================

    [SerializeField]
    private Tilemap TowerTileMap;
    
    [SerializeField]
    private List<TowerData> TowerDatas;

    private Dictionary<TileBase,OreData> oreDataFromTiles;
    private Dictionary<TileBase,FacilityData> facilityDataFromTiles;
    private Dictionary<TileBase,TowerData> towerDataFromTiles;
    private void Awake(){
        //=====================Ore=====================
        oreDataFromTiles = new Dictionary<TileBase,OreData>();

        foreach(OreData data in OreDatas){
            foreach(Tile tile in data.tiles){
                oreDataFromTiles.Add(tile,data);
            }
        }

        //=====================Facility=====================
        facilityDataFromTiles = new Dictionary<TileBase,FacilityData>();

        foreach(FacilityData data in FacilityDatas){
            foreach(Tile tile in data.tiles){
                facilityDataFromTiles.Add(tile,data);
            }
        }

        //=====================Tower=====================
        towerDataFromTiles = new Dictionary<TileBase, TowerData>();

        foreach(TowerData data in TowerDatas){
            foreach(Tile tile in data.tiles){
                towerDataFromTiles.Add(tile,data);
            }
        }
    }

    private void Update(){
        if(Input.GetMouseButton(0)){
            //get the TileMapPos
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = OreTileMap.WorldToCell(mousePos);

            //get the Tile on each map
            TileBase clickedOreTile = OreTileMap.GetTile(gridPos);
            TileBase clickedFacilityTile = FacilityTileMap.GetTile(gridPos);
            TileBase clickedTower = TowerTileMap.GetTile(gridPos);
            bool jumpOff = false;
            if(clickedFacilityTile == null){
                for(int i = -1 ; i <= 1 ; i++){
                    for(int j = -1 ; j <= 1 ; j ++){
                        clickedFacilityTile = FacilityTileMap.GetTile(gridPos + new Vector3Int(i,j,0));
                        if(clickedFacilityTile != null){
                            jumpOff = true;
                        }
                        if(jumpOff){
                            break;
                        }
                    }
                    if(jumpOff){
                        break;
                    }
                }
            }
            //TileBase clickedTileUp = map.GetTile(gridPos + new Vector3Int(0,1));
            //float oreLevel = oreDataFromTiles[clickedOreTile].oreLevel;
            
            //FacilityType type = facilityDataFromTiles[clickedFacilityTile].Facilitytype;
            TowerType towerType = towerDataFromTiles[clickedTower].towerType;
            print("At pos:" + gridPos + " there's a [" + clickedOreTile + "]" + (int)towerType);// + " has oreLevel: " + oreLevel + " FacType:" + type);

        }
    }
}
