using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaRocket : AProjectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            //if (collision.gameObject.tag == "Enemy")
            //    collision.gameObject.GetComponentInParent<AEnemy>().TakeDmg(Dmg);
        }
    }
}
