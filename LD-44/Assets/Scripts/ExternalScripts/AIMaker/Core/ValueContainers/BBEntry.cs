using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    public class BBEntry<T> : ABBEntry, IValueContainerAI<T>
    {
        [fsProperty]
        private T m_value;
        public T Value
        {
            get { return m_value; }
            set
            {
                if (IsValid)
                {
                    m_value = value;
                }
            }
        }

        public BBEntry(BlackBoard bb, string key) : base(bb, key)
        {
            if (Blackboard != null)
            {
                Value = default(T);
            }
        }
        public BBEntry(BlackBoard bb, string key, T value) : base(bb, key)
        {
            if (Blackboard != null)
            {
                Value = value;
            }
        }

        public override Type GetValueType()
        {
            return typeof(T);
        }
        public override object ValueAsObject
        {
            get { return Value; }
            set
            {
                if (!IsValid)
                    return;

                if (value != null)
                {
                    if (typeof(T).IsAssignableFrom(value.GetType()))
                        m_value = (T)value;
                }
                else
                    m_value = default(T);
            }
        }
    }

}