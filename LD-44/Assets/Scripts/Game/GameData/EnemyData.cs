using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public AProjectile ProjectilePrefab;

    public int Start_HP;
    public int Dmg;
    public float Attack_Cd;
    public float Move_Speed;
    public float Attack_Distance;
    public int Money_Reward;
}
