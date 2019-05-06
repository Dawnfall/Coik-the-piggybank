using System;
using System.Collections.Generic;
using UnityEngine;

namespace aim.FullSerializer
{
    partial class fsConverterRegistrar
    {
        public static Internal.DirectConverters.GUIStyle_DirectConverter Register_GUIStyle_DirectConverter;
    }
}

namespace aim.FullSerializer.Internal.DirectConverters
{
    public class GUIStyle_DirectConverter : fsDirectConverter<GUIStyle>
    {
        protected override fsResult DoSerialize(GUIStyle model, Dictionary<string, fsData> serialized, object other)
        {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "active", model.active, other);
            result += SerializeMember(serialized, null, "alignment", model.alignment, other);
            result += SerializeMember(serialized, null, "border", model.border, other);
            result += SerializeMember(serialized, null, "clipping", model.clipping, other);
            result += SerializeMember(serialized, null, "contentOffset", model.contentOffset, other);
            result += SerializeMember(serialized, null, "fixedHeight", model.fixedHeight, other);
            result += SerializeMember(serialized, null, "fixedWidth", model.fixedWidth, other);
            result += SerializeMember(serialized, null, "focused", model.focused, other);
            result += SerializeMember(serialized, null, "font", model.font, other);
            result += SerializeMember(serialized, null, "fontSize", model.fontSize, other);
            result += SerializeMember(serialized, null, "fontStyle", model.fontStyle, other);
            result += SerializeMember(serialized, null, "hover", model.hover, other);
            result += SerializeMember(serialized, null, "imagePosition", model.imagePosition, other);
            result += SerializeMember(serialized, null, "margin", model.margin, other);
            result += SerializeMember(serialized, null, "name", model.name, other);
            result += SerializeMember(serialized, null, "normal", model.normal, other);
            result += SerializeMember(serialized, null, "onActive", model.onActive, other);
            result += SerializeMember(serialized, null, "onFocused", model.onFocused, other);
            result += SerializeMember(serialized, null, "onHover", model.onHover, other);
            result += SerializeMember(serialized, null, "onNormal", model.onNormal, other);
            result += SerializeMember(serialized, null, "overflow", model.overflow, other);
            result += SerializeMember(serialized, null, "padding", model.padding, other);
            result += SerializeMember(serialized, null, "richText", model.richText, other);
            result += SerializeMember(serialized, null, "stretchHeight", model.stretchHeight, other);
            result += SerializeMember(serialized, null, "stretchWidth", model.stretchWidth, other);
            result += SerializeMember(serialized, null, "wordWrap", model.wordWrap, other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyle model, object other)
        {
            var result = fsResult.Success;

            var t0 = model.active;
            result += DeserializeMember(data, null, "active", out t0, other);
            model.active = t0;

            var t2 = model.alignment;
            result += DeserializeMember(data, null, "alignment", out t2, other);
            model.alignment = t2;

            var t3 = model.border;
            result += DeserializeMember(data, null, "border", out t3, other);
            model.border = t3;

            var t4 = model.clipping;
            result += DeserializeMember(data, null, "clipping", out t4, other);
            model.clipping = t4;

            var t5 = model.contentOffset;
            result += DeserializeMember(data, null, "contentOffset", out t5, other);
            model.contentOffset = t5;

            var t6 = model.fixedHeight;
            result += DeserializeMember(data, null, "fixedHeight", out t6, other);
            model.fixedHeight = t6;

            var t7 = model.fixedWidth;
            result += DeserializeMember(data, null, "fixedWidth", out t7, other);
            model.fixedWidth = t7;

            var t8 = model.focused;
            result += DeserializeMember(data, null, "focused", out t8, other);
            model.focused = t8;

            var t9 = model.font;
            result += DeserializeMember(data, null, "font", out t9, other);
            model.font = t9;

            var t10 = model.fontSize;
            result += DeserializeMember(data, null, "fontSize", out t10, other);
            model.fontSize = t10;

            var t11 = model.fontStyle;
            result += DeserializeMember(data, null, "fontStyle", out t11, other);
            model.fontStyle = t11;

            var t12 = model.hover;
            result += DeserializeMember(data, null, "hover", out t12, other);
            model.hover = t12;

            var t13 = model.imagePosition;
            result += DeserializeMember(data, null, "imagePosition", out t13, other);
            model.imagePosition = t13;

            var t16 = model.margin;
            result += DeserializeMember(data, null, "margin", out t16, other);
            model.margin = t16;

            var t17 = model.name;
            result += DeserializeMember(data, null, "name", out t17, other);
            model.name = t17;

            var t18 = model.normal;
            result += DeserializeMember(data, null, "normal", out t18, other);
            model.normal = t18;

            var t19 = model.onActive;
            result += DeserializeMember(data, null, "onActive", out t19, other);
            model.onActive = t19;

            var t20 = model.onFocused;
            result += DeserializeMember(data, null, "onFocused", out t20, other);
            model.onFocused = t20;

            var t21 = model.onHover;
            result += DeserializeMember(data, null, "onHover", out t21, other);
            model.onHover = t21;

            var t22 = model.onNormal;
            result += DeserializeMember(data, null, "onNormal", out t22, other);
            model.onNormal = t22;

            var t23 = model.overflow;
            result += DeserializeMember(data, null, "overflow", out t23, other);
            model.overflow = t23;

            var t24 = model.padding;
            result += DeserializeMember(data, null, "padding", out t24, other);
            model.padding = t24;

            var t25 = model.richText;
            result += DeserializeMember(data, null, "richText", out t25, other);
            model.richText = t25;

            var t26 = model.stretchHeight;
            result += DeserializeMember(data, null, "stretchHeight", out t26, other);
            model.stretchHeight = t26;

            var t27 = model.stretchWidth;
            result += DeserializeMember(data, null, "stretchWidth", out t27, other);
            model.stretchWidth = t27;

            var t28 = model.wordWrap;
            result += DeserializeMember(data, null, "wordWrap", out t28, other);
            model.wordWrap = t28;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType)
        {
            return new GUIStyle();
        }
    }
}