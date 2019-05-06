using System;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer {
    partial class fsConverterRegistrar {
        public static Internal.DirectConverters.Rect_DirectConverter Register_Rect_DirectConverter;
    }
}

namespace aim.FullSerializer.Internal.DirectConverters {
    public class Rect_DirectConverter : fsDirectConverter<Rect> {
        protected override fsResult DoSerialize(Rect model, Dictionary<string, fsData> serialized, object other) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "xMin", model.xMin,other);
            result += SerializeMember(serialized, null, "yMin", model.yMin, other);
            result += SerializeMember(serialized, null, "xMax", model.xMax, other);
            result += SerializeMember(serialized, null, "yMax", model.yMax, other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Rect model, object other) {
            var result = fsResult.Success;

            var t0 = model.xMin;
            result += DeserializeMember(data, null, "xMin", out t0, other);
            model.xMin = t0;

            var t1 = model.yMin;
            result += DeserializeMember(data, null, "yMin", out t1, other);
            model.yMin = t1;

            var t2 = model.xMax;
            result += DeserializeMember(data, null, "xMax", out t2, other);
            model.xMax = t2;

            var t3 = model.yMax;
            result += DeserializeMember(data, null, "yMax", out t3, other);
            model.yMax = t3;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Rect();
        }
    }
}