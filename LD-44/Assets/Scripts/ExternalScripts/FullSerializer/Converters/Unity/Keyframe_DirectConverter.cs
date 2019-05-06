using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim.FullSerializer {
    partial class fsConverterRegistrar {
        public static Internal.DirectConverters.Keyframe_DirectConverter Register_Keyframe_DirectConverter;
    }
}

namespace aim.FullSerializer.Internal.DirectConverters {
    public class Keyframe_DirectConverter : fsDirectConverter<Keyframe> {
        protected override fsResult DoSerialize(Keyframe model, Dictionary<string, fsData> serialized, object other) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "time", model.time, other);
            result += SerializeMember(serialized, null, "value", model.value, other);
            result += SerializeMember(serialized, null, "weightedMode", model.weightedMode, other);
            result += SerializeMember(serialized, null, "inTangent", model.inTangent, other);
            result += SerializeMember(serialized, null, "inWeight", model.inWeight, other);
            result += SerializeMember(serialized, null, "outTangent", model.outTangent, other);
            result += SerializeMember(serialized, null, "outWeight", model.outWeight, other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Keyframe model, object other) {
            var result = fsResult.Success;

            var t0 = model.time;
            result += DeserializeMember(data, null, "time", out t0, other);
            model.time = t0;

            var t1 = model.value;
            result += DeserializeMember(data, null, "value", out t1, other);
            model.value = t1;

            var t2 = model.weightedMode;
            result += DeserializeMember(data, null, "weightedMode", out t2, other);
            model.weightedMode = t2;

            var t3 = model.inTangent;
            result += DeserializeMember(data, null, "inTangent", out t3, other);
            model.inTangent = t3;

            var t4 = model.outTangent;
            result += DeserializeMember(data, null, "outTangent", out t4, other);
            model.outTangent = t4;

            var t5 = model.inWeight;
            result += DeserializeMember(data, null, "inWeight", out t5, other);
            model.inWeight = t5;

            var t6 = model.outWeight;
            result += DeserializeMember(data, null, "outWeight", out t6, other);
            model.outWeight = t6;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Keyframe();
        }
    }
}