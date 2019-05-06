using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using helper;

public class Shotgun : AWeapon
{
    public override bool Shoot(PlayerController player)
    {
        if (ShootCd == 0f)
        {
            float dmgPerRay = Data.Dmg / (float)Data.RayCount;

            Dictionary<AEnemy, int> hitDict = new Dictionary<AEnemy, int>();
            foreach (RaycastHit2D hit in HelperPhysics.Racast2DArk(transform.position, transform.up, Data.Angle, Data.RayCount, 6, false))
            {
                if (hit.collider != null)
                {
                    HelperCommon.AddToDictionary(hitDict, hit.collider.gameObject.GetComponent<AEnemy>(), 1);
                }
            }
            
            foreach(var pair in hitDict)
            {
                print(pair.Key.ToString() + " , " + pair.Value.ToString());
                int totalDmg = (int)(pair.Value * dmgPerRay);
                pair.Key.Life.HP -= totalDmg;
            }

            ShootCd = Data.ShootCd;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //foreach(var dir in HelperMath.GetArkVectors2D(transform.up,Data.Angle,Data.RayCount))
        //{
        //    //Gizmos.DrawRay(transform.position, dir);
        //}
    }
}
