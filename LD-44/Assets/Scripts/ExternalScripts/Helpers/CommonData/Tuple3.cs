﻿using System;

namespace aim
{
    /// <summary>
    /// Represents a functional tuple that can be used to store
    /// two values of different types inside one object.
    /// </summary>
    /// <typeparam name="T1">The type of the first element</typeparam>
    /// <typeparam name="T2">The type of the second element</typeparam>
    /// <typeparam name="T3">The type of the third element</typeparam>
    public sealed class Tuple<T1, T2, T3>
    {
        private readonly T1 item1;
        private readonly T2 item2;
        private readonly T3 item3;

        public T1 Item1
        {
            get { return item1; }
        }
        public T2 Item2
        {
            get { return item2; }
        }
        public T3 Item3
        {
            get { return item3; }
        }

        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (item1 == null ? 0 : item1.GetHashCode());
            hash = hash * 23 + (item2 == null ? 0 : item2.GetHashCode());
            hash = hash * 23 + (item3 == null ? 0 : item3.GetHashCode());
            return hash;
        }
        public override bool Equals(object o)
        {
            Tuple<T1, T2, T3> other = o as Tuple<T1, T2, T3>;

            if (other != null)
                return this == other;
            return false;
        }
        public bool Equals(Tuple<T1, T2, T3> other)
        {
            return this == other;
        }

        public static bool operator ==(Tuple<T1, T2, T3> a, Tuple<T1, T2, T3> b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            if (object.ReferenceEquals(b, null))
                return false;

            if (a.item1 == null && b.item1 != null) return false;
            if (a.item2 == null && b.item2 != null) return false;
            if (a.item3 == null && b.item3 != null) return false;
            return
                a.item1.Equals(b.item1) &&
                a.item2.Equals(b.item2) &&
                a.item3.Equals(b.item3);
        }

        public static bool operator !=(Tuple<T1, T2, T3> a, Tuple<T1, T2, T3> b)
        {
            return !(a == b);
        }

        public void Unpack(Action<T1, T2, T3> unpackerDelegate)
        {
            unpackerDelegate(Item1, Item2, Item3);
        }
    }
}