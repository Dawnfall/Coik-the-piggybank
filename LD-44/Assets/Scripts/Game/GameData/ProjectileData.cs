using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public int Dmg;
    public float Speed;
    public float Splash_Radius;
}
