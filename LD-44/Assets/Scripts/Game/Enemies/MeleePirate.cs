using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePirate : AEnemy
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
            Player.Life.HP -= Data.Dmg;
            CurrAttackCd = Data.Attack_Cd;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Data.Attack_Distance);
    }
}
