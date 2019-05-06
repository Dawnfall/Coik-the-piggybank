using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : AWeapon
{
    [SerializeField] GameObject m_bulletPrefab;

    public override bool Shoot(PlayerController player)
    {
        if (ShootCd == 0f)
        {
            ShootUtility.ShootProjectile(Data.ProjectilePrefab, transform.position, transform.up);
            ShootCd = Data.ShootCd;
            return true;
        }
        return false;
    }
}
