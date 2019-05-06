using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace aim
{
    [System.Serializable]
    public class RegisteredType : IComparable<RegisteredType>
    {
        [SerializeField] private string m_type;
        [SerializeField] private bool m_isUsed = false;
        [SerializeField] private string m_info = "";
        [SerializeField] private string m_createPath = "";

        public RegisteredType(Type type)
        {
            if (type != null)
            {
                m_type = type.AssemblyQualifiedName;
                m_createPath = type.FullName;
            }
            m_isUsed = type.IsPrimitive || typeof(MonoBehaviour).IsAssignableFrom(type);
        }
        public Type Type
        {
            get { return System.Type.GetType(m_type); }
        }
        public bool IsValid
        {
            get
            {
                return Type != null;
            }
        }

        public string Info
        {
            get { return m_info; }
            set { m_info = value; }
        }
        public string CreatePath
        {
            get { return m_createPath; }
            set { m_createPath = value; }
        }
        public bool IsUsed
        {
            get { return m_isUsed; }
            set { m_isUsed = value; }
        }
        public override bool Equals(object obj)
        {
            RegisteredType otherRegType = obj as RegisteredType;

            if (otherRegType != null && otherRegType.Type == Type)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            Type type = Type;
            return (type != null) ? Type.GetHashCode() : 0;
        }

        public static bool operator ==(RegisteredType a, RegisteredType b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            else if (object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(RegisteredType a, RegisteredType b)
        {
            return !(a == b);

        }
        public static bool operator true(RegisteredType a)
        {
            return a != null;
        }
        public static bool operator false(RegisteredType a)
        {
            return a == null;
        }

        public int CompareTo(RegisteredType other)
        {
            return m_type.CompareTo(other.m_type);
        }

    }
}