using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LaserWeaponData",menuName = "Custom/TileDatas/WeaponDatas/LaserWeaponData")]
public class laserWeaponData : ScriptableObject
{
    //damage/s
    public float atkDamage;
    //draws laser
    public LineRenderer laser;
}
