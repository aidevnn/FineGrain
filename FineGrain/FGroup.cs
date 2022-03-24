using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public abstract class GElt<T> : Elt<T>, IComparable<GElt<T>> where T : struct, IEquatable<T>, IComparable<T>
    {
        protected GElt(FSet<T> fSet, int hash) : base(fSet, hash) { }

        private int order;
        public int Order
        {
            get => order;
            set
            {
                if (order != 0 || value < 1)
                    return;

                order = value;
            }
        }

        public int CompareTo(GElt<T> other)
        {
            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            return base.CompareTo(other);
        }
    }

    public class GEltComparer<T, U> : Comparer<U> where U : GElt<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        public override int Compare(U x, U y) => x.CompareTo(y);
    }

    public abstract class FGroup<T, U> : Table<T> where U : GElt<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        protected FGroup(int hash) : base(hash) { }
        protected FGroup(int[] dims) : base(dims) { }

        protected abstract U Create(params T[] ts);
        protected abstract U DefineOp(U a, U b);

        protected void CreateCaches(int size)
        {
            cache0 = new T[size];
            cache1 = new T[size];
            cache2 = new T[size];
        }

        protected void AddElt(U e)
        {
            if (!FSetContains(e.HashCode))
                FSetAdd(e);

            if (e.Order < 1)
                Monogenic(e);
        }

        U InternalOp(U a, U b)
        {
            if (TableOpContains(a.HashCode, b.HashCode))
                return GetElement<U>(TableOp(a.HashCode, b.HashCode));

            var e = DefineOp(a, b);
            TableOpAdd(a.HashCode, b.HashCode, e.HashCode);
            if (!FSetContains(e.HashCode))
                FSetAdd(e);
            else
                return GetElement<U>(e.HashCode);

            return e;
        }

        public int evilCounter;
        void Monogenic(U e) => Monogenic(e, Identity);
        void Monogenic(U e, U acc0, int ord = 1)
        {
            ++evilCounter;
            if (e.Order > 0)
                return;

            var acc1 = InternalOp(e, acc0);
            if (acc1.HashCode == Identity.HashCode)
            {
                e.Order = ord;
                return;
            }

            Monogenic(e, acc1, ord + 1);

            if (acc1.Order < 1)
                Monogenic(acc1);
        }

        public U CreateElement(params T[] ts)
        {
            var p = Create(ts);
            AddElt(p);
            return p;
        }

        public U Identity { get; private set; }
        public void CreateIdentity(U id)
        {
            Identity = id;
            AddElt(Identity);
        }

        public U Op(U a, U b)
        {
            var e = InternalOp(a, b);
            AddElt(e);
            return e;
        }

        public U Invert(U e) => GetElement<U>(GetInvert(e.HashCode));

        public IEnumerable<U> Elements() => Elements<U>();
    }
}
