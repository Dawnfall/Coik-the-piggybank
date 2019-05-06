using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShootUtility 
{
    public static void ShootProjectile(AProjectile projectilePrefab, Vector3 pos, Vector3 dir)
    {
        AProjectile newProjectile = GameObject.Instantiate(projectilePrefab).GetComponent<AProjectile>();

        newProjectile.transform.position = pos;
        newProjectile.transform.up = dir;
        newProjectile.Rigidbody.velocity = dir * newProjectile.Data.Speed;
    }
}
