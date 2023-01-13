using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType{
    Gun,
    Missle,
    laser

};
[CreateAssetMenu (fileName = "WeaponData",menuName = "Custom/TileDatas/WeaponData")]
public class WeaponData : ScriptableObject
{
    public WeaponType weaponType;

    public float atkDamage;
    public float atkSpeed;
    public float bulletSpeed;
    public float bulletLifeSpan;
    public GameObject bullet;
}
