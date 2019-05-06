using System;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer {
    partial class fsConverterRegistrar {
        public static Internal.DirectConverters.Bounds_DirectConverter Register_Bounds_DirectConverter;
    }
}

namespace aim.FullSerializer.Internal.DirectConverters {
    public class Bounds_DirectConverter : fsDirectConverter<Bounds> {
        protected override fsResult DoSerialize(Bounds model, Dictionary<string, fsData> serialized, object other) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "center", model.center, other);
            result += SerializeMember(serialized, null, "size", model.size, other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Bounds model, object other) {
            var result = fsResult.Success;

            var t0 = model.center;
            result += DeserializeMember(data, null, "center", out t0, other);
            model.center = t0;

            var t1 = model.size;
            result += DeserializeMember(data, null, "size", out t1, other);
            model.size = t1;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Bounds();
        }
    }
}