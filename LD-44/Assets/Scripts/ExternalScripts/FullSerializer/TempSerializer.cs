using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer
{
    public class TempSerializer : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<int> values = new List<int>();
        [SerializeField]
        List<UnityEngine.Object> keys = new List<UnityEngine.Object>();

        Dictionary<UnityEngine.Object, int> objToIntDict = new Dictionary<UnityEngine.Object, int>();
        Dictionary<int, UnityEngine.Object> intToObjDict = new Dictionary<int, UnityEngine.Object>();

        public int AddObject(UnityEngine.Object unityObject)
        {
            if (unityObject == null)
                return -1;

            if (objToIntDict.ContainsKey(unityObject))
                return objToIntDict[unityObject];

            objToIntDict.Add(unityObject, objToIntDict.Count);
            intToObjDict.Add(objToIntDict.Count, unityObject);
            return objToIntDict.Count - 1;
        }

        public UnityEngine.Object GetObject(int index)
        {
            UnityEngine.Object unityObject;
            intToObjDict.TryGetValue(index, out unityObject);
            return unityObject;
        }

        public void OnBeforeSerialize()
        {
            values = new List<int>(objToIntDict.Values);
            keys = new List<UnityEngine.Object>(objToIntDict.Keys);
        }
        public void OnAfterDeserialize()
        {
            objToIntDict.Clear();
            intToObjDict.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                objToIntDict.Add(keys[i], values[i]);
                intToObjDict.Add(values[i], keys[i]);
            }
        }
    }
}