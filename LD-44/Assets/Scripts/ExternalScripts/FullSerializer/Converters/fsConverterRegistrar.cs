using System;
using System.Collections.Generic;
using aim.FullSerializer.Internal;

namespace aim.FullSerializer
{
    /// <summary>
    /// This class allows arbitrary code to easily register global converters. To
    /// add a converter, simply declare a new field called "Register_*" that
    /// stores the type of converter you would like to add. Alternatively, you
    /// can do the same with a method called "Register_*"; just add the converter
    /// type to the `Converters` list.
    /// </summary>
    public partial class fsConverterRegistrar
    {
        static fsConverterRegistrar()
        {
            Converters = new List<Type>();

            foreach (var field in typeof(fsConverterRegistrar).GetDeclaredFields())
            {
                if (field.Name.StartsWith("Register_"))
                    Converters.Add(field.FieldType);
            }

            foreach (var method in typeof(fsConverterRegistrar).GetDeclaredMethods())
            {
                if (method.Name.StartsWith("Register_"))
                    method.Invoke(null, null);
            }

            // Make sure we do not use any AOT Models which are out of date.
            var finalResult = new List<Type>(Converters);
            foreach (Type t in Converters)
            {
                object instance = null;
                try
                {
                    instance = Activator.CreateInstance(t);
                }
                catch (Exception) { }

                var aotConverter = instance as fsIAotConverter;
                if (aotConverter != null)
                {
                    var modelMetaType = fsMetaType.Get(new fsConfig(), aotConverter.ModelType);
                    if (fsAotCompilationManager.IsAotModelUpToDate(modelMetaType, aotConverter) == false)
                    {
                        finalResult.Remove(t);
                    }
                }
            }
            Converters = finalResult;
        }

        public static List<Type> Converters;

        public static void Register_Vector4Converter()
        {
            Converters.Add(typeof(Vector4_DirectConverter));
        }
        public static void Register_Vector3Converter()
        {
            Converters.Add(typeof(Vector3_DirectConverter));
        }
        public static void Register_Vector2Converter()
        {
            Converters.Add(typeof(Vector2_DirectConverter));
        }
        public static void Register_UnityObjectConverter()
        {
            Converters.Add(typeof(UnityObjectConverter));
        }
        //public static void Register_GraphAIConverter()
        //{
        //    Converters.Add(typeof(fsGraphAIConverter));
        //}
        //public static void Register_BBEntryConverter()
        //{
        //    Converters.Add(typeof(BBEntryConverter));
        //}

        // Example field registration:
        //public static AnimationCurve_DirectConverter Register_AnimationCurve_DirectConverter;

        // Example method registration:
        //public static void Register_AnimationCurve_DirectConverter() {
        //    Converters.Add(typeof(AnimationCurve_DirectConverter));
        //}
    }
}