using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "RingWeaponData",menuName = "Custom/TileDatas/WeaponDatas/RingWeaponData")]
public class ringWeaponData : ScriptableObject
{
    //damage/s
    public float atkDamage;
    public float atkSpeed;
    public float ringScaleSpeed;
    public float lifespan;
    //draws Ring
    public LineRenderer ring;
    public List<ParticleSystem> particleSystemList;//prefab
    //atkRange = MaxRadius = ringScaleSpeed * lifespan
    private float atkRangeSquared;
    public void setAtkRangeSquared(float atkRangeSquared){
        this.atkRangeSquared = atkRangeSquared;
    }
    public float getAtkRangeSquared(){
        return atkRangeSquared;
    }
}