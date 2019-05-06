using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using aim.FullSerializer;
using aim.FullSerializer.Internal;
namespace aim
{

    public static class PropertyAISerializer
    {
        private static fsConfig m_config = new fsConfig();

        public static void DrawDefaultPropertiesAI(object instance, AIAgent aiAgent)
        {
            if (instance == null)
                return;

            fsMetaType metaType = fsMetaType.Get(m_config, instance.GetType());
            foreach (var prop in metaType.Properties)
            {
                if (fsPortableReflection.HasAttribute(prop.MemberInfo, typeof(HideInInspector), false))
                {
                    continue;
                }
                DrawPropertyAI(instance, prop, aiAgent);
            }
        }

        public static void DrawPropertyAI(object instance, fsMetaProperty property, AIAgent aiAgent)
        {
            if (instance == null || property == null)
                return;
            property.Write(instance, DrawInstance(property.Read(instance), property.StorageType, property.MemberName, aiAgent));
        }
        public static void DrawPropertyAI(object instance, string propName, AIAgent aiAgent)
        {
            if (instance == null)
                return;
            Type instanceType = instance.GetType();

            FieldInfo field = instanceType.GetField(propName);
            if (field != null)
            {
                DrawPropertyAI(instance, new fsMetaProperty(m_config, field), aiAgent);
                return;
            }

            PropertyInfo prop = instanceType.GetProperty(propName);
            if (prop != null)
            {
                DrawPropertyAI(instance, new fsMetaProperty(m_config, prop), aiAgent);
                return;
            }

        }

        /// <summary>
        /// draws instance with appropriate drawer in inspectorAI
        /// </summary>
        /// <param name="instance">instance to be drawn</param>
        /// <param name="instanceType">instance type that will determine drawer</param>
        /// <param name="label">optional label</param>
        /// <param name="aiAgent">aiAgent this instance is a part of</param>
        /// <returns> modified instance </returns>
        public static object DrawInstance(object instance, Type instanceType, string label, AIAgent aiAgent)
        {
            if (instanceType == null)
            {
                return instance;
            }
            IPropertyDrawer drawer;
            //if (instanceType.IsArray || typeof(List<>).IsAssignableFrom(instanceType))
            //{

            //}
            //else if (typeof(IList).IsAssignableFrom(instanceType))
            //{
            //    drawer = new ListPropertyAIDrawer();
            //}
            //else if (typeof(IDictionary).IsAssignableFrom(instanceType))
            //    drawer = new DictionaryPropertyAIDrawer();
            //else
            drawer = ConfigAI_E.Instance.GetPropertyDrawer(instanceType);

            if (drawer != null)
            {
                return drawer.DrawProperty(instance, instanceType, label, aiAgent);
            }
            else
            {
                return instance;

            }
        }

        private static IList DrawList(IList instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return instance;
        }
        private static IDictionary DrawDictionary(IDictionary instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return instance;
        }
        private static object DrawArray(object instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            return instance;
            //instanceType.IsArray
            //for (int i = 0; i <)
        }

        //public static void DrawProperty(object instance, string propertyName, AIAgent aiAgent)
        //{

        //}



        //public static void DrawProperty(object instance, string fieldName, AIAgent aiAgent)
        //{
        //    DrawProperty(instance, instance.GetType().GetField(fieldName), aiAgent);
        //}


        //public static void DrawProp(object instance, PropertyInfo propInfo, AIAgent aiAgent)
        //{
        //    if (propInfo.CanRead && propInfo.CanWrite)
        //}

    }

}