using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_UI : MonoBehaviour
{
    [SerializeField] Vector3 m_startPosition;
    [SerializeField] PanelAwakeType m_panelStartType = PanelAwakeType.NONE;

    protected virtual void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = m_startPosition;

        switch (m_panelStartType)
        {
            case PanelAwakeType.ACTIVATE:
                Activate();
                break;
            case PanelAwakeType.DEACTIVATE:
                Deactivate();
                break;
        }
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

public enum PanelAwakeType
{
    ACTIVATE,
    DEACTIVATE,
    NONE
}