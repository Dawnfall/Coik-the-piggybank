using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace aim.FullSerializer
{
    public class UnityObjectConverter : fsConverter
    {
        public override bool CanProcess(Type type)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(type);
        }
        public override object CreateInstance(fsData data, Type storageType)
        {
            return null;
        }

        public override bool RequestCycleSupport(Type storageType)
        {
            return false;
        }
        public override bool RequestInheritanceSupport(Type storageType)
        {
            return base.RequestInheritanceSupport(storageType);
        }

        public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType, object other)
        {
            IUnityObjectRegister unityObjectRegister = other as IUnityObjectRegister; //TODO: maybe? do interface for this
            if (unityObjectRegister != null) 
            {
                instance = unityObjectRegister.GetRegisteredUO((int)data.AsInt64);
                return fsResult.Success;
            }

            Debug.Log("Error deserializing UO!");
            return fsResult.Fail("");
        }
        public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType, object other)
        {
            IUnityObjectRegister unityObjectRegister = other as IUnityObjectRegister; //TODO: maybe? do interface for this
            if (unityObjectRegister != null)
            {
                serialized = new fsData(unityObjectRegister.RegisterUnityObject(instance as UnityEngine.Object));
                return fsResult.Success;
            }
            serialized = new fsData(-1);

            Debug.Log("Error serializing UO!");
            return fsResult.Fail("");
        }
    }

    public interface IUnityObjectRegister
    {
        int RegisterUnityObject(UnityEngine.Object uo);
        UnityEngine.Object GetRegisteredUO(int index);
    }
}

