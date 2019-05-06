using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeElement
{
    private int m_hp;

    public UnityEvent OnDieEvent = new UnityEvent();
    public UnityEvent OnHpChangeEvent = new UnityEvent();

    public int HP
    {
        get { return m_hp; }
        set
        {
            if (m_hp == value)
                return;

            m_hp = value;
            OnHpChangeEvent.Invoke();

            if (m_hp <= 0)
            {
                m_hp = 0;
                OnDieEvent.Invoke();
            }

        }
    }

}
