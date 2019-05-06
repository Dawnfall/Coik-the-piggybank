using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using helper;

public abstract class AEnemy : MonoBehaviour
{
    public EnemyData Data;

    PlayerController m_player;
    public PlayerController Player
    {
        get
        {
            if (!m_player)
                m_player = FindObjectOfType<PlayerController>();
            return m_player;
        }
    }

    Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody
    {
        get
        {
            if (!m_rigidbody)
                m_rigidbody = GetComponent<Rigidbody2D>();
            return m_rigidbody;
        }
    }

    public virtual AudioSource Audio
    {
        get { return GetComponent<AudioSource>(); }
    }

    //****************
    // Elements

    public LifeElement Life= new LifeElement();

    //***************
    // Properties

    public float CurrAttackCd
    {
        get; protected set;
    }

    //**********
    // Pipeline

    protected virtual void Start()
    {
        Life.HP = Data.Start_HP;
        Life.OnDieEvent.AddListener(Die);
    }

    protected virtual void Update()
    {
        HandleCds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
            Life.HP -= collision.gameObject.GetComponentInParent<AProjectile>().Data.Dmg;
    }

    //*************
    // Internal

    protected abstract void Attack();

    //**************
    // Usable

    public virtual bool IsAbleToAttack
    {
        get { return HelperPhysics.GetDistanceFromTo(transform, Player.transform, true) <= Data.Attack_Distance * Data.Attack_Distance; }
    }
    public virtual void TryMoveToPlayer()
    {
        if (!IsAbleToAttack)
        {
            Vector3 dir = (Player.transform.position - transform.position).normalized;
            Rigidbody.velocity = dir * Data.Move_Speed;
        }
        else
            Rigidbody.velocity = Vector3.zero;
    }
    public virtual void TryToAttack()
    {
        if (IsAbleToAttack)
        {
            Attack();
        }
    }
    public virtual void HandleCds()
    {
        CurrAttackCd = Mathf.Max(0, CurrAttackCd - Time.deltaTime);
    }

    public virtual void Die()
    {
        Player.Life.HP += Data.Money_Reward;
        Audio.Play();
        Destroy(gameObject, 0.2f);
    }

    //*************
    // Debug

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Data.Attack_Distance);
    }
}
