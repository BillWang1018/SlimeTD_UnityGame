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


    private Dictionary<TileBase,OreData> oreDataFromTiles;
    private Dictionary<TileBase,FacilityData> facilityDataFromTiles;
    private Dictionary<TileBase,TowerData> towerDataFromTiles;

    //tower Locs
    private List<Vector3Int> towerLocs;

    //Towers --> Weapons --> time
    private Dictionary<Vector3Int,Dictionary<gunWeaponData,float>> gunWeaponTimers;
    private Dictionary<Vector3Int,Dictionary<laserWeaponData,float>> laserWeaponTimers;
    private Dictionary<Vector3Int,Dictionary<laserWeaponData,LineRenderer>> laserWeaponRenderers;
    //=====================UI image place=====================
    public Tilemap UItileMap;
    private TowerData currentSelectedTowerData = null;
    private void Awake(){
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

        //=====================All the Tower Data store=====================
        towerDataFromTiles = new Dictionary<TileBase, TowerData>();

        foreach(TowerData data in TowerDatas){
            foreach(Tile tile in data.tiles){
                towerDataFromTiles.Add(tile,data);
            }
        }
        //add tower timer
        towerLocs = new List<Vector3Int>();

        gunWeaponTimers = new Dictionary<Vector3Int, Dictionary<gunWeaponData, float>>();
        laserWeaponTimers = new Dictionary<Vector3Int, Dictionary<laserWeaponData, float>>();
        laserWeaponRenderers = new Dictionary<Vector3Int, Dictionary<laserWeaponData, LineRenderer>>();
        //for each tower pos get tile
        foreach(Vector3Int v in TowerTileMap.cellBounds.allPositionsWithin){
            TileBase t = TowerTileMap.GetTile(v);
            Vector3 worldLoc = TowerTileMap.CellToWorld(v);
            //if there's a tower
            if(t != null){
                towerLocs.Add(v);
                //add the gunWeaponData in to Dic
                
                
                //for each weapon add to childDictionary  
                
                if(towerDataFromTiles[t].gunWeaponDatas != null){
                    Dictionary<gunWeaponData, float> gunWeaponTimeDic = new Dictionary<gunWeaponData, float>();
                    foreach(gunWeaponData wd in towerDataFromTiles[t].gunWeaponDatas){
                        gunWeaponTimeDic.Add(wd,0.0f);
                    }
                    gunWeaponTimers.Add(v,gunWeaponTimeDic);
                }
                if(towerDataFromTiles[t].laserWeaponDatas != null){
                    Dictionary<laserWeaponData, float> laserWeaponTimeDic = new Dictionary<laserWeaponData, float>();
                    Dictionary<laserWeaponData, LineRenderer> laserRendererDic = new Dictionary<laserWeaponData, LineRenderer>();
                    foreach(laserWeaponData lw in towerDataFromTiles[t].laserWeaponDatas){
                        print("laser Placed! " + lw.atkDamage);
                        laserWeaponTimeDic.Add(lw,0.0f);
                        laserRendererDic.Add(lw,Instantiate(lw.laser,Vector3.zero,Quaternion.identity));
                    }
                    laserWeaponTimers.Add(v,laserWeaponTimeDic);
                    laserWeaponRenderers.Add(v,laserRendererDic);
                }
            }
        }

        //ImgListDrag
    }
    public GameObject inventory;
    private void Update(){
        //update UI
        PlaceUItower();

        //right click to cancel place tower
        if(Input.GetMouseButtonDown(1) && !inventory.activeSelf && inventory.GetComponent<TowerMenuState>().state){
            UItileMap.SetTile(lastUIGridPos,null);
            currentSelectedTowerData = null;
            inventory.SetActive(true);
        }

        //left click to place tower
        if(Input.GetMouseButton(0) && currentSelectedTowerData != null){
            
            if(!towerLocs.Contains(currentUIGridPos)){
                placeTower(currentUIGridPos,currentSelectedTowerData);
            }
        }

        //update tower
        foreach(Vector3Int loc in towerLocs){
            TileBase towerTile = TowerTileMap.GetTile(loc);  
            Vector3 worldLoc = TowerTileMap.CellToWorld(loc);

            GameObject nearestEnemy = getNearestEnemy(worldLoc);
            
            //if no enemy
            if(nearestEnemy == null)break;

            Vector3 EnemyPos = nearestEnemy.transform.position;
            

            //make tower rotate
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


            //every weapon's func
            if(towerDataFromTiles[towerTile].gunWeaponDatas != null){
                foreach(gunWeaponData wd in towerDataFromTiles[towerTile].gunWeaponDatas){
                    //have n weapon + n timer
                    gunWeaponTimers[loc][wd] += Time.deltaTime;
                    //shoot
                    if(gunWeaponTimers[loc][wd] >= wd.atkSpeed){
                        gunWeaponTimers[loc][wd] -= wd.atkSpeed;

                        GameObject b = Instantiate(wd.bullet,worldLoc + anchor,rotation);
                        b.GetComponent<Bullet>().setBulletAtk(wd.atkDamage);
                        b.GetComponent<Bullet>().setBulletSpeed(wd.bulletSpeed);
                        b.GetComponent<Bullet>().setBulletLifeSpan(wd.bulletLifeSpan);
                            
                            
                    }
                } 
            }

            if(towerDataFromTiles[towerTile].laserWeaponDatas != null){
                foreach(laserWeaponData wd in towerDataFromTiles[towerTile].laserWeaponDatas){
                    //have n weapon + n timer
                    laserWeaponTimers[loc][wd] += Time.deltaTime;
                    nearestEnemy.GetComponent<PathFollower>().Health -= wd.atkDamage * Time.deltaTime;
                    //draw laser
                    laserWeaponRenderers[loc][wd].enabled = true;
                    laserWeaponRenderers[loc][wd].SetPosition(0,worldLoc);
                    laserWeaponRenderers[loc][wd].SetPosition(1,EnemyPos);
                    //wd.laser.transform.position = worldLoc;
                    
                }
            }
        }
    }
    GameObject getNearestEnemy(Vector3 pos){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");
        if(enemies.Length == 0)return null;

        Vector3 resPos = new Vector3(0.0f,0.0f,0.0f);
        GameObject resObject = enemies[0];
        float minDis = float.MaxValue;
        float dis = 0.0f;
        foreach(GameObject g in enemies){
            dis = getDisSquared(g.transform.position,pos);
            if(dis < minDis){
                minDis = dis;
                resObject = g;
            }
        }
        return resObject;
        
    }
    Vector3 getNearestEnemyPos(Vector3 pos){
        GameObject g = getNearestEnemy(pos);
        return g.transform.position;
    }

    float getDisSquared(Vector3 pos1,Vector3 pos2){
        return (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y);
    }

    public void placeTower(Vector3Int pos,TowerData td){
        //if the tower data only have 1 tile ,this code will work
        TowerTileMap.SetTile(pos,td.tiles[0]);
        towerLocs.Add(pos);
        Vector3 worldLoc = TowerTileMap.CellToWorld(pos);
        Dictionary<gunWeaponData, float> gunWeaponTimeDic = new Dictionary<gunWeaponData, float>();
        Dictionary<laserWeaponData, float> laserWeaponTimeDic = new Dictionary<laserWeaponData, float>();
        Dictionary<laserWeaponData,LineRenderer> laserWeaponRendererDic = new Dictionary<laserWeaponData, LineRenderer>();
        //for each weapon add to Dictionary  
        if(td.gunWeaponDatas != null){
            foreach(gunWeaponData wd in td.gunWeaponDatas){
                gunWeaponTimeDic.Add(wd,0.0f);
            }
        }
        if(td.laserWeaponDatas != null){
            foreach(laserWeaponData wd in td.laserWeaponDatas){
                laserWeaponTimeDic.Add(wd,0.0f);
                laserWeaponRendererDic.Add(wd,Instantiate(wd.laser,Vector3.zero,Quaternion.identity));
            }
        }
        gunWeaponTimers.Add(pos,gunWeaponTimeDic);
        laserWeaponTimers.Add(pos,laserWeaponTimeDic);
        laserWeaponRenderers.Add(pos,laserWeaponRendererDic);
    }

    public void setCurrentSelectedData(TowerData td){
        currentSelectedTowerData = td;
    }

    Vector3Int lastUIGridPos = Vector3Int.zero;
    Vector3Int currentUIGridPos = Vector3Int.zero;
    public void PlaceUItower(){//doesn't func ,just a virtual view
        if(currentSelectedTowerData == null){
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        currentUIGridPos = UItileMap.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos));
        currentUIGridPos.z = 0;
        //if the tile changes
        if(lastUIGridPos != currentUIGridPos){
            UItileMap.SetTile(currentUIGridPos,currentSelectedTowerData.tiles[0]);
            UItileMap.SetTile(lastUIGridPos,null);

            //last pos = curn pos
            lastUIGridPos = currentUIGridPos;
        }
    }
}