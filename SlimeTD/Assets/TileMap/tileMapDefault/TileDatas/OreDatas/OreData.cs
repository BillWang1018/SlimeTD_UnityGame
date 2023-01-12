using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName = "OreData",menuName = "Custom/TileDatas/OreData")]
public class OreData : ScriptableObject
{
    public TileBase[] tiles;
    public int oreLevel;
}
