using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO:...make MonoSingleton , for editor create output option

//#if UNITY_EDITOR
//[SerializeField] private bool m_doCheck = true;
//[SerializeField] private List<string> m_eventNames;

//#if UNITY_EDITOR
//        if (m_doCheck)
//        {
//            if (!m_eventNames.Contains(eventName))
//            {
//                Debug.Log("Starting unregistered event: " + eventName + ", action: " + action.ToString());
//                return;
//            }
//        }
//#endif

//    #if UNITY_EDITOR
//        if (m_doCheck)
//        {
//            if (!m_eventNames.Contains(eventName))
//            {
//                Debug.Log("Starting unregistered event: " + eventName+ ", action: " +action.ToString());
//                return;
//            }
//        }
//#endif

//    #if UNITY_EDITOR
//        if (m_doCheck)
//        {
//            if (!m_eventNames.Contains(eventName))
//            {
//                Debug.Log("Starting unregistered event: " + eventName);
//                return;
//            }
//        }
//#endif

//#endif

public class EventManager : ASingleton<EventManager>
{
    private Dictionary<EventName, UnityEventArg> m_eventDict = new Dictionary<EventName, UnityEventArg>();

    public static void StartListening(EventName eventName, UnityAction<object> action)
    {
        EventManager evManager = Instance;

        UnityEventArg ev = null;
        evManager.m_eventDict.TryGetValue(eventName, out ev);
        if (ev == null)
            evManager.m_eventDict.Add(eventName, ev = new UnityEventArg());

        ev.AddListener(action);
    }
    public static void StopListening(EventName eventName, UnityAction<object> action)
    {
        EventManager evManager = Instance;

        UnityEventArg ev = null;
        evManager.m_eventDict.TryGetValue(eventName, out ev);
        if (ev == null)
            return;
        ev.RemoveListener(action);
    }

    public static void TriggerEvent(EventName eventName)
    {
        TriggerEvent(eventName, null);
    }
    public static void TriggerEvent(EventName eventName, object param)
    {
        EventManager evManager = Instance;

        UnityEventArg ev = null;
        evManager.m_eventDict.TryGetValue(eventName, out ev);

        if (ev != null)
            ev.Invoke(param);
    }
}

[System.Serializable]
public class UnityEventArg : UnityEvent<object> { }

public enum EventName
{
    Game_Started,
    Game_Resumed,

    Game_Points_Changed,
    Game_TimeLeft_Changed,
    Game_Level_Changed,
    Game_Speed_Changed,

    Game_Over,

    Game_Combination_Done
}