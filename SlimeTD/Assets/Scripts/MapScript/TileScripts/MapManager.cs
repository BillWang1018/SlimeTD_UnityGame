using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
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

    //tower Locs
    private List<Vector3Int> towerLocs;

    //Towers --> Weapons --> time
    private Dictionary<Vector3Int,Dictionary<WeaponData,float>> weaponTimers;
    //=====================UI image Drag=====================
    [SerializeField]
    private List<Image> imgList;
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
        //add tower timer
        towerLocs = new List<Vector3Int>();

        weaponTimers = new Dictionary<Vector3Int, Dictionary<WeaponData, float>>();
        //for each tower pos get tile
        foreach(Vector3Int v in TowerTileMap.cellBounds.allPositionsWithin){
            TileBase t = TowerTileMap.GetTile(v);
            //if there's a tower
            if(t != null){
                towerLocs.Add(v);
                Dictionary<WeaponData, float> WeaponTimeDic = new Dictionary<WeaponData, float>();
                //for each weapon add to Dictionary  
                foreach(TowerData td in TowerDatas){
                    foreach(WeaponData wd in td.weaponDatas){
                        WeaponTimeDic.Add(wd,0.0f);
                    }
                }
                weaponTimers.Add(v,WeaponTimeDic);
            }
        }

        //ImgListDrag
        

    }
    
    private void Update(){
        //Vector3 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3Int gridPos = OreTileMap.WorldToCell(Pos);
        
        //update tower
        foreach(Vector3Int loc in towerLocs){
            TileBase towerTile = TowerTileMap.GetTile(loc);  
            Vector3 worldLoc = TowerTileMap.CellToWorld(loc);

            //make tower rotate
            Vector3 EnemyPos = getNearestEnemyPos(worldLoc);
            if(EnemyPos == Vector3.zero){continue;}
            Vector3 v = EnemyPos - worldLoc;
            float rotz = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg + 90.0f;

            Quaternion rotation = Quaternion.Euler(0,0,rotz);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);
            
            TileChangeData tcData = new TileChangeData{
                position = loc,
                tile = towerTile,
                color = Color.white,
                transform = rotMatrix

            };
            
            TowerTileMap.SetTile(tcData,false);
            Vector3 anchor = new Vector3(0.16f,0.16f);
            foreach(WeaponData wd in towerDataFromTiles[towerTile].weaponDatas){
                //have n weapon + n timer
                weaponTimers[loc][wd] += Time.deltaTime;
                //shoot
                if(weaponTimers[loc][wd] >= wd.atkSpeed){
                    weaponTimers[loc][wd] = 0;

                    GameObject b = Instantiate(wd.bullet,worldLoc + anchor,rotation);
                    b.GetComponent<Bullet>().setBulletAtk(wd.atkDamage);
                    b.GetComponent<Bullet>().setBulletSpeed(wd.bulletSpeed);
                    b.GetComponent<Bullet>().setBulletLifeSpan(wd.bulletLifeSpan);
                        
                        
                }
            } 
        }
        /*if(Input.GetMouseButton(0)){
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
        */
    }
    Vector3 getNearestEnemyPos(Vector3 pos){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");
        if(enemies.Length == 0)return new Vector3(0.0f,0.0f,0.0f);

        Vector3 resPos = new Vector3(0.0f,0.0f,0.0f);
        float minDis = float.MaxValue;
        float dis = 0.0f;
        foreach(GameObject g in enemies){
            dis = getDisSquared(g.transform.position,pos);
            if(dis < minDis){
                minDis = dis;
                resPos = g.transform.position;
            }
        }
        return resPos;
        
    }

    float getDisSquared(Vector3 pos1,Vector3 pos2){
        return (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z);
    }
}
