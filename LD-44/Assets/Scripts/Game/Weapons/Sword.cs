using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : AWeapon
{
    public override bool Shoot(PlayerController player)
    {
        if (ShootCd == 0f)
        {

            //TODO:::.......
            ShootCd = ShootCd;

            return true;
        }
        return false;
    }
}
