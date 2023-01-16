using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum FacilityType{
    techTower,
    techEco

};
[CreateAssetMenu (fileName = "OreData",menuName = "Custom/TileDatas/FacilityData")]
public class FacilityData : ScriptableObject
{
    public TileBase[] tiles;
    public FacilityType Facilitytype;

}
