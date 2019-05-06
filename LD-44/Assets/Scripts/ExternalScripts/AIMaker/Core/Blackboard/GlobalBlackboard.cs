using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    [ExecuteInEditMode]
    public class GlobalBlackboard : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static GlobalBlackboard m_globalBlackboard;
        public static GlobalBlackboard Instance
        {
            get
            {
                if (m_globalBlackboard == null)
                {
                    GameObject bbGO = new GameObject("Global BB");
                    DontDestroyOnLoad(bbGO);

                    m_globalBlackboard = bbGO.AddComponent<GlobalBlackboard>();
                }
                return m_globalBlackboard;
            }
        }

        private BlackBoard m_bb = new BlackBoard();
        public BlackBoard Blackboard
        {
            get { return m_bb; }
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }
    }
}