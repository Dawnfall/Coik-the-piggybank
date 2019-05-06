using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASingleton<T> where T : new()
{
    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new T();
            return m_instance;
        }
    }
}