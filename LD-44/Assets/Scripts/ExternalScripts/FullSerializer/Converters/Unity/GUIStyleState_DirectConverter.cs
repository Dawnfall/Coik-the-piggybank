using System;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer
{
    partial class fsConverterRegistrar
    {
        public static Internal.DirectConverters.GUIStyleState_DirectConverter Register_GUIStyleState_DirectConverter;
    }
}

namespace aim.FullSerializer.Internal.DirectConverters
{
    public class GUIStyleState_DirectConverter : fsDirectConverter<GUIStyleState>
    {
        protected override fsResult DoSerialize(GUIStyleState model, Dictionary<string, fsData> serialized, object other)
        {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "background", model.background, other);
            result += SerializeMember(serialized, null, "textColor", model.textColor,other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyleState model, object other)
        {
            var result = fsResult.Success;

            var t0 = model.background;
            result += DeserializeMember(data, null, "background", out t0, other);
            model.background = t0;

            var t2 = model.textColor;
            result += DeserializeMember(data, null, "textColor", out t2, other);
            model.textColor = t2;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType)
        {
            return new GUIStyleState();
        }
    }
}