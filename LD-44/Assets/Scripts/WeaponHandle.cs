using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandle : MonoBehaviour
{
    private AWeapon m_activeWeapon;
    public AWeapon ActiveWeapon
    {
        get
        {
            return m_activeWeapon;
        }
        set
        {
            if (value == m_activeWeapon)
                return;
            if (m_activeWeapon != null)
                m_activeWeapon.Hide();
            m_activeWeapon = value;
            if (m_activeWeapon != null)
                m_activeWeapon.Show();
        }
    }

    public AWeapon[] GetAllWeapons()
    {
        return transform.GetComponentsInChildren<AWeapon>();
    }

    public void HideAllWeapons()
    {
        foreach (AWeapon weapon in GetAllWeapons())
            weapon.Hide();
        m_activeWeapon = null;
    }

    public void SetWeapon(int index)
    {
        if (index < 0 || index >= transform.childCount)
            return;

        AWeapon[] allWeapons = GetAllWeapons();
        ActiveWeapon = allWeapons[index];
    }
    public int GetIndexOfCurrentActive()
    {
        if (!ActiveWeapon)
            return -1;
        else
            return ActiveWeapon.transform.GetSiblingIndex();
    }
    public void SetNextWeapon()
    {
        SetWeapon((GetIndexOfCurrentActive() + 1) % transform.childCount);
    }
    public void SetPreviousWeapon()
    {
        SetWeapon((GetIndexOfCurrentActive() - 1 + transform.childCount) % transform.childCount);
    }

}
