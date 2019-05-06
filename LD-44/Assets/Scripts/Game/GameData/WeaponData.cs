using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public AProjectile ProjectilePrefab;

    public int Dmg;
    public float ShootCd;
    public int MoneyCost;

    public float Angle;
    public int RayCount;
}
