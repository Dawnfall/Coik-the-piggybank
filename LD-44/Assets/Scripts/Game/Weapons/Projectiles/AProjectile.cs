using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public ProjectileData Data;

    private Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody
    {
        get
        {
            if (!m_rigidbody)
                m_rigidbody = GetComponent<Rigidbody2D>();
            return m_rigidbody;
        }
    }


    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
