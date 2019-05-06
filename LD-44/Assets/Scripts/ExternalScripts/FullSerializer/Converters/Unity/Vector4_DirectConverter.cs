using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace aim.FullSerializer
{
    public class Vector4_DirectConverter : fsDirectConverter<UnityEngine.Vector4>
    {
        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector4 model, object other)
        {
            float[] vecArr;
            DeserializeMember<float[]>(data, null, "arr", out vecArr, other);

            if (vecArr == null || vecArr.Length != 4)
            {
                return fsResult.Fail("incorrect deserialization data for Vector3!");
            }

            model.x = vecArr[0];
            model.y = vecArr[1];
            model.z = vecArr[2];
            model.w = vecArr[3];

            return fsResult.Success;
        }

        protected override fsResult DoSerialize(Vector4 model, Dictionary<string, fsData> serialized, object other)
        {
            float[] vecArr = new float[4] { model.x, model.y, model.z, model.w };

            SerializeMember<float[]>(serialized, null, "arr", vecArr, other);

            return fsResult.Success;
        }
    }
}