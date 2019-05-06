using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using helper;

public class KnifePirate : AEnemy
{
    protected override void Update()
    {
        base.Update();
        TryToAttack();
    }

    protected void FixedUpdate()
    {
        TryMoveToPlayer();
    }

    protected override void Attack()
    {
        if (IsAbleToAttack)
        {
            ShootUtility.ShootProjectile(Data.ProjectilePrefab, transform.position, HelperPhysics.GetDirFromTo(transform, Player.transform, true));
            CurrAttackCd = Data.Attack_Cd;
        }
    }
}
