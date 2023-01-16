using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TowerType{
    tower0,
    tower1,
    tower2,
    tower3,
    tower4,
    tower5,
    tower6,
    tower7,
    tower8,
    tower9,
};

[CreateAssetMenu (fileName = "TowerData",menuName = "Custom/TileDatas/TowerData")]
public class TowerData : ScriptableObject
{
    public TileBase[] tiles;
    public TowerType towerType;
    public List<gunWeaponData> gunWeaponDatas;
    public List<laserWeaponData> laserWeaponDatas;
    public List<ringWeaponData> ringWeaponDatas;
}
