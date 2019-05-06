using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    public WeaponData Data;

    public virtual SpriteRenderer Renderer
    {
        get { return GetComponentInChildren<SpriteRenderer>(); }
    }
    public virtual AudioSource Audio
    {
        get { return GetComponent<AudioSource>(); }
    }

    public float ShootCd
    {
        get;
        protected set;
    }

    protected virtual void OnEnable()
    {
        ShootCd = 0f;
    }
    protected virtual void Update()
    {
        ShootCd = Mathf.Max(0, ShootCd - Time.deltaTime);
    }
    public abstract bool Shoot(PlayerController player);

    public void Hide()
    {
        Renderer.enabled = false;
    }
    public void Show()
    {
        Renderer.enabled = true;
    }

}
