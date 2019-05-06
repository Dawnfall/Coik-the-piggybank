using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer
{
    public class Vector2_DirectConverter : fsDirectConverter<UnityEngine.Vector2>
    {
        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector2 model, object other)
        {
            float[] vecArr;
            DeserializeMember<float[]>(data, null, "arr", out vecArr, other);

            if (vecArr == null || vecArr.Length != 2)
                return fsResult.Fail("incorrect deserialization data for Vector2!");

            model.x = vecArr[0];
            model.y = vecArr[1];

            return fsResult.Success;
        }

        protected override fsResult DoSerialize(Vector2 model, Dictionary<string, fsData> serialized, object other)
        {
            float[] vecArr = new float[2] { model.x, model.y };

            SerializeMember<float[]>(serialized, null, "arr", vecArr, other);

            return fsResult.Success;
        }
    }
}