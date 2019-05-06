using System.Reflection;
using System;

namespace aim
{
    public abstract class APropertyAIDrawer<T> : IPropertyDrawer
    {
        /// <summary>
        /// Abstract method that draws a certain type in AIMaker environment ; this is how T should be presented when encountered in inspector
        /// </summary>
        /// <param name="label"> label that will be used </param>
        /// <param name="instance"> current value of a field that is to be edited </param>
        /// <param name="aiAgent"> parent agentAI </param>
        /// <returns> Method should return the modified value of an instance </returns>
        public abstract T Draw(T instance, System.Type instanceType, string label, AIAgent aiAgent);

        public object DrawProperty(object instance, System.Type instanceType, string label, AIAgent aiAgent)
        {
            if (instanceType == null)
                return instance;
            if (instance == null)
                return Draw(default(T), instanceType, label, aiAgent);
            return Draw((T)instance, instanceType, label, aiAgent);
        }

        ///// <summary>
        ///// This method defines how the actual property of this type will be presented
        ///// </summary>
        ///// <param name="objectWithProperty"> object that property belongs to </param>
        ///// <param name="fieldInfo"> field </param>
        ///// <param name="aiAgent">parent AgentAI</param>
        //public virtual void DrawProperty(object objectWithProperty, FieldInfo fieldInfo, AIAgent aiAgent) //maybe null problems
        //{
        //    if (objectWithProperty == null || fieldInfo == null)
        //        return;

        //    object propertyValue = fieldInfo.GetValue(objectWithProperty);
        //    if (propertyValue is T)
        //        fieldInfo.SetValue(objectWithProperty, Draw(fieldInfo.Name, (T)propertyValue, aiAgent));
        //}
    }
}
