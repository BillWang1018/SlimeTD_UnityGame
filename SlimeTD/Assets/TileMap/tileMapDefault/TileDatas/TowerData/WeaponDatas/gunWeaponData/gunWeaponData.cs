using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "GunWeaponData",menuName = "Custom/TileDatas/WeaponDatas/GunWeaponData")]
public class gunWeaponData : ScriptableObject{
    //damage deals to the enemy
    public float atkDamage;
    //s/bullet
    public float atkSpeed;
    //speed/s
    public float bulletSpeed;
    //how long will the bullet disappear itself
    public float bulletLifeSpan;
    //the bullet it shoot
    public GameObject bullet;

    
}
