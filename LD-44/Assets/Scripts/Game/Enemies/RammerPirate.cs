using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class RammerPirate : AEnemy //TODO:::.......
//{
//    private float m_distanceToGoal;
//    private Vector3 ramGoal;

//    private bool m_isRamming = false;
//    private bool IsRamming
//    {
//        get { return m_isRamming; }
//        set
//        {
//            if (m_isRamming == value)
//                return;

//            m_isRamming = value;
//            if (!m_isRamming)
//                CurrAttackCd = s_AttackCd;
//        }
//    }

//    protected override void HandleMove()
//    {
//        if (IsRamming)
//        {
//            if (Vector3.Distance(transform.position, ramGoal) < 0.05f)
//            {
//                IsRamming = false;
//                Rigidbody.velocity = Vector2.zero;
//            }
//        }
//    }
//    protected override void Attack()
//    {
//        if(m_distanceToGoal==0)
//        {
//            ramGoal = Player.transform.position;
//            m_distanceToGoal = Vector3.Distance(transform.position, ramGoal);
//        }

//    }
//}
