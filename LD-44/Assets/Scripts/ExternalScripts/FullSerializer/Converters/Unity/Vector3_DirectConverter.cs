using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace aim.FullSerializer
{
    public class Vector3_DirectConverter : fsDirectConverter<UnityEngine.Vector3>
    {
        //public override object CreateInstance(fsData data, Type storageType)
        //{
        //    return base.CreateInstance(data, storageType);
        //}
        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector3 model, object other)
        {
            float[] vecArr;
            DeserializeMember<float[]>(data, null, "arr", out vecArr, other);

            if (vecArr == null || vecArr.Length != 3)
            {
                return fsResult.Fail("incorrect deserialization data for Vector3!");
            }

            model.x = vecArr[0];
            model.y = vecArr[1];
            model.z = vecArr[2];

            return fsResult.Success;
        }

        protected override fsResult DoSerialize(Vector3 model, Dictionary<string, fsData> serialized, object other)
        {
            float[] vecArr = new float[3] { model.x, model.y, model.z };

            SerializeMember<float[]>(serialized, null, "arr", vecArr, other);

            return fsResult.Success;
        }
    }

}