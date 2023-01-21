using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

using TMPro;
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
    private Dictionary<Vector3Int,Dictionary<gunWeaponData,GunDataStorer>> gunDataStorage;
    //laser
    private Dictionary<Vector3Int,Dictionary<laserWeaponData,LaserDataStorer>> laserWeaponStorage;
    //Rings
    private Dictionary<Vector3Int,Dictionary<ringWeaponData,RingDataStorer>> ringDataStorage;
    private Dictionary<ringWeaponData,ParticleSystem> ringParticleEffect;

    public class GunDataStorer{
        public float time = 0.0f;
        public List<GameObject> bulletpool = new List<GameObject>();
        public int currentBulletIndex = 0;
    };
    public class LaserDataStorer{
        public float time = 0.0f;
        public LineRenderer lineRenderer;
    }
    public class RingDataStorer{
        //the ring tower's timer
        public float time = 0.0f;
        public List<LineRenderer> LineRenderers = new List<LineRenderer>(); 
        public List<float> LineRenderRadius = new List<float>();
        public List<float> LineRenderTime = new List<float>();
        public int nextIndexToEnable = 0;
        //ring -> enemy
        public List<List<GameObject>> encounteredEnemies = new List<List<GameObject>>();
        
    }

    
    //=====================UI image place=====================
    public Tilemap UItileMap;
    private TowerData currentSelectedTowerData = null;
    //InfoBox
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

        //=====================All the Tower Data store=====================
        towerDataFromTiles = new Dictionary<TileBase, TowerData>();
        ringParticleEffect = new Dictionary<ringWeaponData, ParticleSystem>();
        //!-modify the data-!
        foreach(TowerData data in TowerDatas){
            //===========gun=========set atkRangeSquared
            for(int i = 0 ; i < data.gunWeaponDatas.Count ; i++){
                float x = data.gunWeaponDatas[i].bulletLifeSpan * data.gunWeaponDatas[i].bulletSpeed;
                data.gunWeaponDatas[i].setAtkRangeSquared(x * x);
            }
            //===========laser========set atkRangeSquared
            for(int i = 0 ; i < data.laserWeaponDatas.Count ; i++){
                data.laserWeaponDatas[i].setAtkRangeSquared(data.laserWeaponDatas[i].atkRange * data.laserWeaponDatas[i].atkRange);
            }
            //===========ring=========add Praticles data
            for(int i = 0 ; i < data.ringWeaponDatas.Count ; i++){
                for(int  j = 0 ; j < data.ringWeaponDatas[i].particleSystemList.Count ; j++){
                    ringParticleEffect.Add(data.ringWeaponDatas[i],Instantiate(data.ringWeaponDatas[i].particleSystemList[j]));
                }
                //set ringweapondata's atkRange(According to the lifespan and ScaleSpeed)
                float x = data.ringWeaponDatas[i].lifespan * data.ringWeaponDatas[i].ringScaleSpeed;
                data.ringWeaponDatas[i].setAtkRangeSquared(x * x);
            }

            foreach(Tile tile in data.tiles){
                towerDataFromTiles.Add(tile,data);
            }
            
        }
        //add tower timer
        towerLocs = new List<Vector3Int>();

        gunDataStorage = new Dictionary<Vector3Int, Dictionary<gunWeaponData, GunDataStorer>>();
        laserWeaponStorage = new Dictionary<Vector3Int, Dictionary<laserWeaponData, LaserDataStorer>>();
        ringDataStorage = new Dictionary<Vector3Int, Dictionary<ringWeaponData, RingDataStorer>>();
        //the storage to store the info inside infobox
        infoBoxDataStorage = new Dictionary<Vector3Int, infoBoxClass>();
        //for each tower pos get tile
        foreach(Vector3Int v in TowerTileMap.cellBounds.allPositionsWithin){
            TileBase t = TowerTileMap.GetTile(v);
            Vector3 worldLoc = TowerTileMap.CellToWorld(v);
            //if there's a tower
            if(t != null){
                placeTower(v,towerDataFromTiles[t]);
            }
        }
        
    }
    public GameObject inventory;
    
    Vector3 anchor = new Vector3(0.16f,0.16f);
    private void Update(){
        //infoBox update
        ClickedTileTower();
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
            
            Vector3 EnemyPos = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Vector3 v = Vector3.zero;
            if(nearestEnemy != null){
                EnemyPos = nearestEnemy.transform.position;
            
                //make tower rotate
                v = EnemyPos - (worldLoc + anchor);
                float rotz = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg + 90.0f;

                rotation = Quaternion.Euler(0,0,rotz);
                Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);
                
                TileChangeData tcData = new TileChangeData{
                    position = loc,
                    tile = towerTile,
                    color = Color.white,
                    transform = rotMatrix

                };
                
                TowerTileMap.SetTile(tcData,false);
            }
            Vector3 worldLocCenter = worldLoc + anchor;
            //every weapon's func
            if(towerDataFromTiles[towerTile].gunWeaponDatas != null){
                foreach(gunWeaponData wd in towerDataFromTiles[towerTile].gunWeaponDatas){

                    //check if the bullet hit
                    foreach(GameObject b in gunDataStorage[loc][wd].bulletpool){
                        if(wd.bulletLifeSpan < b.GetComponent<Bullet>().getLife()){
                            b.SetActive(false);
                        }
                        if(!b.activeSelf)continue;
                        //if active then check colli
                        foreach(GameObject g in currentEnemies){
                            //if colli
                            if(getDisSquared(g.transform.position,b.transform.position) <= 0.8){
                                b.SetActive(false);

                                if(g.GetComponent<DefaultEnemyBehavior>().health - wd.atkDamage >= 0){
                                    infoBoxDataStorage[loc].totalDamageDealt += wd.atkDamage;
                                }else{
                                    infoBoxDataStorage[loc].totalDamageDealt += g.GetComponent<DefaultEnemyBehavior>().health;
                                }

                                g.GetComponent<DefaultEnemyBehavior>().health -= wd.atkDamage;

                                


                                if(g.GetComponent<DefaultEnemyBehavior>().health <= 0){
                                    Destroy(g);
                                }

                                

                            } 
                        }
                    }

                    //have n weapon + n timer
                    gunDataStorage[loc][wd].time += Time.deltaTime;
                    //if no enemy (or not in the range) then no shoot
                    if(nearestEnemy == null || getDisSquared(EnemyPos,worldLocCenter) > wd.getAtkRangeSquared())break;
                    //shoot
                    if(gunDataStorage[loc][wd].time >= wd.atkSpeed){
                        gunDataStorage[loc][wd].time = 0;
                        int index = gunDataStorage[loc][wd].currentBulletIndex;
                        GameObject b = gunDataStorage[loc][wd].bulletpool[index];
                        b.transform.position = worldLocCenter;
                        b.transform.rotation = rotation;
                        //gunDataStorge[loc][wd].bulletpool
                        //GameObject b = Instantiate(wd.bullet,worldLocCenter,rotation);
                        b.GetComponent<Bullet>().setBulletAtk(wd.atkDamage);
                        b.GetComponent<Bullet>().setBulletSpeed(wd.bulletSpeed);
                        b.GetComponent<Bullet>().setBulletLifeSpan(wd.bulletLifeSpan);
                        b.GetComponent<Bullet>().setIniDirection(v.normalized);
                        b.GetComponent<Bullet>().resetLife();
                            
                        b.SetActive(true);

                        gunDataStorage[loc][wd].currentBulletIndex++;
                        if(gunDataStorage[loc][wd].currentBulletIndex >= gunDataStorage[loc][wd].bulletpool.Count){
                            gunDataStorage[loc][wd].currentBulletIndex = 0;
                        }
                    }
                } 
            }
            //laser weapon func
            if(towerDataFromTiles[towerTile].laserWeaponDatas != null){
                foreach(laserWeaponData wd in towerDataFromTiles[towerTile].laserWeaponDatas){
                    //have n weapon + n timer
                    laserWeaponStorage[loc][wd].time += Time.deltaTime;
                    
                    //no enemy(or not in the range) then no laser rendered
                    if(nearestEnemy == null || getDisSquared(EnemyPos,worldLocCenter) > wd.getAtkRangeSquared()){   
                        laserWeaponStorage[loc][wd].lineRenderer.enabled = false;
                        continue;
                    }

                    if(nearestEnemy.GetComponent<DefaultEnemyBehavior>().health - wd.atkDamage >= 0){
                        infoBoxDataStorage[loc].totalDamageDealt += wd.atkDamage;
                    }else{
                        infoBoxDataStorage[loc].totalDamageDealt += nearestEnemy.GetComponent<DefaultEnemyBehavior>().health;
                    }

                    nearestEnemy.GetComponent<DefaultEnemyBehavior>().health -= wd.atkDamage * Time.deltaTime;
                    if(nearestEnemy.GetComponent<DefaultEnemyBehavior>().health <= 0){
                        Destroy(nearestEnemy);
                    }
                    //draw laser
                    
                    laserWeaponStorage[loc][wd].lineRenderer.enabled = true;
                    laserWeaponStorage[loc][wd].lineRenderer.SetPosition(0,worldLocCenter);
                    laserWeaponStorage[loc][wd].lineRenderer.SetPosition(1,EnemyPos);
                    //wd.laser.transform.position = worldLoc;
                    
                }
            }
            //ring weapon func
            if(towerDataFromTiles[towerTile].ringWeaponDatas != null){
                foreach(ringWeaponData wd in towerDataFromTiles[towerTile].ringWeaponDatas){
                    //have n weapon + n timer
                    ringDataStorage[loc][wd].time += Time.deltaTime;
                    //shoot ring (add new renderer)
                    if(ringDataStorage[loc][wd].time >= wd.atkSpeed && nearestEnemy != null && getDisSquared(nearestEnemy.transform.position,worldLocCenter) < wd.getAtkRangeSquared()){
                        ringDataStorage[loc][wd].time = 0;

                        int index = ringDataStorage[loc][wd].nextIndexToEnable;
                        ringDataStorage[loc][wd].LineRenderRadius[index] = 0.0f;
                        ringDataStorage[loc][wd].LineRenderTime[index] = 0.0f;

                        for(int j = 0 ; j < ringDataStorage[loc][wd].LineRenderers[index].positionCount ; j++){
                            ringDataStorage[loc][wd].LineRenderers[index].SetPosition(j,worldLocCenter);
                        }

                        ringDataStorage[loc][wd].LineRenderers[index].enabled = true;
                        ringDataStorage[loc][wd].nextIndexToEnable++;
                        //out of index bounds
                        //print("ringDataStorage[loc][wd].nextIndexToEnable:" + index);
                        if(ringDataStorage[loc][wd].nextIndexToEnable == ringDataStorage[loc][wd].LineRenderers.Count){
                            ringDataStorage[loc][wd].nextIndexToEnable = 0;
                        }
                        break;
                         
                    }
                    
                    for(int i = 0 ; i < ringDataStorage[loc][wd].LineRenderers.Count ;i ++){
                        if(!ringDataStorage[loc][wd].LineRenderers[i].enabled){
                            continue;
                        }
                        //update ring time
                        ringDataStorage[loc][wd].LineRenderTime[i] += Time.deltaTime;
                        //if time is up then set unenable
                        if(ringDataStorage[loc][wd].LineRenderTime[i] >= wd.lifespan){
                            ringDataStorage[loc][wd].LineRenderers[i].enabled = false;
                            ringDataStorage[loc][wd].LineRenderRadius[i] = 0.0f;
                            ringDataStorage[loc][wd].LineRenderTime[i] = 0.0f;
                            ringDataStorage[loc][wd].encounteredEnemies[i].Clear();
                            //print("afterClearCount : " + ringDataStorage[loc][wd].encounteredEnemies[i].Count);
                            continue;
                        }
                        
                        //update ring radius and draw stuff
                        ringDataStorage[loc][wd].LineRenderRadius[i] += wd.ringScaleSpeed * Time.deltaTime;
                        float radius = ringDataStorage[loc][wd].LineRenderRadius[i];
                        //
                        LineRenderer lr = ringDataStorage[loc][wd].LineRenderers[i]; 
                        float perAngle = 2f * Mathf.PI / ((float)lr.positionCount);
                        float curnAngle = 0f;
                        
                        for(int j = 0 ; j < lr.positionCount ; j++){
                            //cos(radians) and sin(radians)
                            curnAngle += perAngle; 
                            lr.SetPosition(j,(new Vector3(Mathf.Cos(curnAngle)* radius,Mathf.Sin(curnAngle) * radius) + worldLocCenter));
                        }
                        
                        //update the damage to the enemy
                        if(nearestEnemy != null){//if there's enemy then check if enemy colli with the ring
                            Vector3 towerToEnemyVector = Vector3.zero;
                            Vector3 tempV = Vector3.zero;
                            foreach(GameObject g in currentEnemies){
                                towerToEnemyVector = g.transform.position - (worldLocCenter);
                                towerToEnemyVector.z = 0;
                                tempV = worldLocCenter + towerToEnemyVector.normalized * radius;
                                tempV.z = 0;
                                //if hit
                                if(getDisSquared(tempV,g.transform.position) <= 0.8f && !ringDataStorage[loc][wd].encounteredEnemies[i].Contains(g)){
                                    g.GetComponent<DefaultEnemyBehavior>().health -= wd.atkDamage;
                                    if(g.GetComponent<DefaultEnemyBehavior>().health <= 0){
                                        Destroy(g);
                                    }
                                    ringDataStorage[loc][wd].encounteredEnemies[i].Add(g);
                                    
                                    //practice test
                                    ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
                                    ep.position = g.transform.position;
                                    //ep.angularVelocity = 0.0f;
                                    //ep.startLifetime = 3.0f;
                                    //ep.velocity = new Vector3(-5 + Random.value * 10, -5 + Random.value * 10);
                                    ep.applyShapeToPosition = true;
                                    //ep.startSize = 3.0f;

                                    ParticleSystem.MainModule emain = ringParticleEffect[wd].main;
                                    

                                    ParticleSystem.EmissionModule em = ringParticleEffect[wd].emission;
                                    //em.enabled = false;
                                    //em.rateOverTime = 200;
                                    ringParticleEffect[wd].Emit(ep,10);
                                    
                                    
                                }
                            }
                        }
                    }
                } 
            }
        }
    }
    
    GameObject[] currentEnemies;
    GameObject getNearestEnemy(Vector3 pos){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("MovingDude");
        currentEnemies = enemies;
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
        return ((pos1.x - pos2.x) * (pos1.x - pos2.x)) + ((pos1.y - pos2.y) * (pos1.y - pos2.y));
    }

    public void placeTower(Vector3Int pos,TowerData td){
        //if the tower data only have 1 tile ,this code will work
        TowerTileMap.SetTile(pos,td.tiles[0]);
        towerLocs.Add(pos);
        Vector3 worldLoc = TowerTileMap.CellToWorld(pos);
        
       
        //for each weapon add to Dictionary  
        if(td.gunWeaponDatas != null){
            Dictionary<gunWeaponData,GunDataStorer> gunWeaponDic = new Dictionary<gunWeaponData, GunDataStorer>();
            foreach(gunWeaponData wd in td.gunWeaponDatas){
                GunDataStorer gds = new GunDataStorer();
                int bulletCountNeeded = (int)(wd.bulletLifeSpan / wd.atkSpeed) + 1;
                for(int i = 0 ; i < bulletCountNeeded ; i++){
                    GameObject bullet = Instantiate(wd.bullet);
                    bullet.SetActive(false);

                    bullet.GetComponent<Bullet>().setBulletLifeSpan(wd.bulletLifeSpan);
                    bullet.GetComponent<Bullet>().setIniDirection(Vector3.zero);
                    bullet.GetComponent<Bullet>().setBulletSpeed(wd.bulletSpeed);

                    gds.bulletpool.Add(bullet);
                }
                gunWeaponDic.Add(wd,gds);
            }
            gunDataStorage.Add(pos,gunWeaponDic);
        }
        if(td.laserWeaponDatas != null){
            Dictionary<laserWeaponData, LaserDataStorer> laserWeaponDic = new Dictionary<laserWeaponData, LaserDataStorer>();
            foreach(laserWeaponData wd in td.laserWeaponDatas){

                LaserDataStorer lds = new LaserDataStorer();
                lds.time = 0.0f;
                lds.lineRenderer = Instantiate(wd.laser,Vector3.zero,Quaternion.identity);
                laserWeaponDic.Add(wd,lds);
            }
            laserWeaponStorage.Add(pos,laserWeaponDic);
        }
        if(td.ringWeaponDatas != null){
            Dictionary<ringWeaponData, RingDataStorer> ringWeaponDic = new Dictionary<ringWeaponData, RingDataStorer>();
            foreach(ringWeaponData wd in td.ringWeaponDatas){
                RingDataStorer rds = new RingDataStorer();
                int LineRenderersNeed = (int)(wd.lifespan / wd.atkSpeed) + 1;
                for(int i = 0 ; i < LineRenderersNeed ; i++){
                    LineRenderer linerenderer = Instantiate(wd.ring,Vector3.zero,Quaternion.identity);
                    linerenderer.positionCount = 50;
                    rds.LineRenderers.Add(linerenderer);
                    rds.LineRenderRadius.Add(0.0f);
                    rds.LineRenderTime.Add(0.0f);
                    rds.encounteredEnemies.Add(new List<GameObject>());
                }

                ringWeaponDic.Add(wd,rds);
            }
            ringDataStorage.Add(pos,ringWeaponDic);
        }
        
        infoBoxDataStorage.Add(pos,new infoBoxClass());
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


    public GameObject infoBox;
    Dictionary<Vector3Int,infoBoxClass> infoBoxDataStorage;
    public class infoBoxClass{
        public float DPS = 0.0f;
        public float totalDamageDealt = 0.0f;
    }
    public void ClickedTileTower(){//update the info box to get the towerdata
        if(Input.GetMouseButtonDown(0)){
            Vector3 mouseToWorldLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseToWorldLoc.z = 0;
            Vector3Int gridPos = TowerTileMap.WorldToCell(mouseToWorldLoc);
            Sprite img = TowerTileMap.GetSprite(gridPos);
            TileBase tb = TowerTileMap.GetTile(gridPos);
            
            print("pos:" + gridPos);

            if(tb != null){
                TowerData td = towerDataFromTiles[TowerTileMap.GetTile(gridPos)];
            

                infoBox.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = img;//icon
                infoBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = td.towerType.ToString();//towerName
                infoBox.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "DPS:1";//dps
                infoBox.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "Damage Dealt:2";//damage dealt
            }
        }
    }
}