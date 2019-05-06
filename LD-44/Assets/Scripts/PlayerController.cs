using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using helper;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Text m_hpText;

    private Transform m_crossHair;
    public Transform CrossHair
    {
        get
        {
            if (!m_crossHair)
                m_crossHair = HelperUnity.findComponentOnChild<Transform>("Crosshair", gameObject);
            return m_crossHair;
        }
    }

    private WeaponHandle m_weaponHandle;
    public WeaponHandle WeaponHandle
    {
        get
        {
            if (!m_weaponHandle)
                m_weaponHandle = GetComponentInChildren<WeaponHandle>();
            return m_weaponHandle;
        }
    }

    private Rigidbody2D m_rigidbody;
    public Rigidbody2D RigidBody
    {
        get
        {
            if (!m_rigidbody)
                m_rigidbody = GetComponent<Rigidbody2D>();
            return m_rigidbody;
        }
    }

    private CameraController m_cameraController;
    public CameraController CameraController
    {
        get
        {
            if (!m_cameraController)
                m_cameraController = FindObjectOfType<CameraController>();
            return m_cameraController;
        }
    }

    //*****************
    // Elements

    public LifeElement Life = new LifeElement();

    //*****************
    // Properties


    private void Start()
    {
        int startHp = GameManager.Instance.SavedHealth;
        Life.HP = (startHp < 0) ? _StartHP : startHp;

        Life.OnDieEvent.AddListener(Die);
        Life.OnHpChangeEvent.AddListener(() => { m_hpText.text = "HP = " + Life.HP; });

        WeaponHandle.HideAllWeapons();
        WeaponHandle.SetNextWeapon();
    }
    private void Update()
    {
        HandleWeaponSwitch();
        HandleShoot();

        HandleCrosshair();
        HandleWeaponLookAt();
    }
    private void FixedUpdate()
    {
        HandleMove();
    }

    //*******************
    // Usable
    //*******************

    public Vector2 AimDir
    {
        get
        {
            return (CrossHair.transform.position - transform.position).normalized;
        }
    }

    //*******************
    // Internal
    //*******************

    private void HandleWeaponSwitch()
    {
        if (Input.GetAxisRaw("Weapon0") == 1)
            WeaponHandle.SetWeapon(0);
        else if (Input.GetAxisRaw("Weapon1") == 1)
            WeaponHandle.SetWeapon(1);
        else if (Input.GetAxisRaw("Weapon2") == 1)
            WeaponHandle.SetWeapon(2);
        else if (Input.GetAxisRaw("Weapon3") == 1)
            WeaponHandle.SetWeapon(3);


        else if (Input.GetAxisRaw("Scroll") == 1)
            WeaponHandle.SetNextWeapon();
        else if (Input.GetAxisRaw("Scroll") == -1)
            WeaponHandle.SetPreviousWeapon();
    }

    private void HandleShoot()
    {
        if (Input.GetAxisRaw("Shoot") == 1)
        {
            AWeapon weapon = WeaponHandle.ActiveWeapon;
            if (weapon != null)
                if (weapon.Shoot(this))
                {
                    weapon.Audio.Play();
                    Life.HP -= weapon.Data.MoneyCost;
                }
        }
    }
    private void HandleCrosshair()
    {
        Vector3 newCrossHairPos = CameraController.GetMouseWorldPosition();
        newCrossHairPos.z = -1;

        CrossHair.position = newCrossHairPos;
    }
    private void HandleWeaponLookAt()
    {
        Vector3 lookAtDir = CrossHair.position - WeaponHandle.transform.position;
        lookAtDir.z = 0;

        WeaponHandle.transform.up = lookAtDir;
    }
    private void HandleMove()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        float speed = (Input.GetAxisRaw("Run") == 1) ? _RunSpeed : _WalkSpeed;

        Vector3 moveVector = inputDir * speed;
        RigidBody.velocity = moveVector;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LooseScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            Life.HP -= collision.gameObject.GetComponentInParent<AProjectile>().Data.Dmg;
        }
    }

    //*******************
    // Statics

    public const int _StartHP = 1000;
    public const float _RunSpeed = 10;
    public const float _WalkSpeed = 7;
}
