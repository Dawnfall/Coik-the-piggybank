using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool m_doDestroyOnLoad = true;

    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<T>();

                if (m_instance == null)
                {
                    string name = typeof(T).ToString();

                    GameObject newGO = new GameObject(name);
                    m_instance = newGO.AddComponent<T>();

                }
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this as T;
        else if (m_instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        if (m_doDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    protected virtual void OnDestroy() //this may be a problem
    {
        if (m_instance == this)
            m_instance = null;
    }

    protected virtual void OnAwake()
    {

    }
}

//T[] prefabs = Resources.LoadAll<T>("");
//if (prefabs.Length > 0)
//{
//    m_instance = Instantiate(prefabs[0]);
//}