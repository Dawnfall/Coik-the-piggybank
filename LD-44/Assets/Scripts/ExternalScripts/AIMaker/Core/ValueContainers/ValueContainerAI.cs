using System;

namespace aim
{
    public abstract class AValueContainerAI : IValueContainerAI
    {
        public abstract object ValueAsObject
        {
            get;
            set;
        }
        public abstract Type GetValueType();
    }



    public class ValueContainerAI<T> : AValueContainerAI, IValueContainerAI<T>
    {
        [FullSerializer.fsProperty] T m_value = default(T);
        public T Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public override object ValueAsObject
        {
            get { return m_value; }
            set
            {
                if (value == null)
                    Value = default(T);
                else if (value is T)
                    Value = (T)value;
            }
        }
        public override Type GetValueType()
        {
            return typeof(T);
        }
    }
}
