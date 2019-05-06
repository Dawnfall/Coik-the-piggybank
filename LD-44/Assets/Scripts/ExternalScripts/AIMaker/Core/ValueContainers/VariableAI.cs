using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public abstract class AVariableAI : IValueContainerAI
    {
        public abstract Type GetValueType();
        public abstract bool IsBound { get; }
        public abstract void Bind(ABBEntry bbEntry);
        public abstract void UnBind();
        public abstract object ValueAsObject { get; set; }
        public abstract ABBEntry BBEntry { get; }
    }

    public class VariableAI<T> : AVariableAI, IValueContainerAI<T>
    {
        [FullSerializer.fsProperty] IValueContainerAI<T> m_valueContainer = new ValueContainerAI<T>();

        public T Value
        {
            get
            {
                return m_valueContainer.Value;
            }
            set
            {
                m_valueContainer.Value = value;
            }
        }

        public override void Bind(ABBEntry bbEntry)
        {
            if (bbEntry != null)
            {
                BBEntry<T> bbEntryT = bbEntry as BBEntry<T>;
                if (bbEntryT != null)
                    m_valueContainer = bbEntryT;
            }
        }
        public void Bind(BBEntry<T> bbEntry)
        {
            if (bbEntry != null)
                m_valueContainer = bbEntry;
        }
        public override void UnBind()
        {
            if (m_valueContainer is ABBEntry)
                m_valueContainer = new ValueContainerAI<T>();
        }

        public override bool IsBound
        {
            get
            {
                return m_valueContainer is ABBEntry;
            }
        }
        public override ABBEntry BBEntry
        {
            get
            {
                return m_valueContainer as ABBEntry;
            }
        }

        public override object ValueAsObject
        {
            get { return Value; }
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

