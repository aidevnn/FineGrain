using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public abstract class AObj : IEquatable<AObj>
    {
        public int HashCode { get; }

        protected AObj(int hash)
        {
            HashCode = hash;
        }

        public bool Equals(AObj other) => HashCode == other.HashCode;
    }

    public abstract class FSet<T> : AObj where T : struct
    {
        public readonly Dictionary<int, AObj> elts = new Dictionary<int, AObj>();

        protected FSet(int hash) : base(hash) { CreateCaches(1); }

        protected T[] cache0, cache1, cache2;
        public int CacheLength => cache0.Length;
        protected void FSetAdd(AObj obj) => elts[obj.HashCode] = obj;

        public string FmtElt { get; protected set; } = "({0})[{1}]";
        public string Fmt { get; protected set; } = "|{0}| = {1}";

        public bool FSetContains(int hash) => elts.ContainsKey(hash);
        public V GetElement<V>(int hash) where V : AObj => (V)elts[hash];
        public IEnumerable<V> Elements<V>() where V : AObj => elts.Values.Cast<V>();

        public override int GetHashCode() => base.GetHashCode();

        public void ClearCaches()
        {
            for (int k = 0; k < cache0.Length; ++k)
                cache0[k] = cache1[k] = cache2[k] = default;
        }

        protected void CreateCaches(int size)
        {
            cache0 = new T[size];
            cache1 = new T[size];
            cache2 = new T[size];
        }

    }

    public abstract class Elt<T> : AObj, IEquatable<Elt<T>> , IComparable<Elt<T>> where T : struct, IEquatable<T>, IComparable<T>
    {
        protected T[] table;

        protected Elt(FSet<T> fSet, int hash) : base(hash)
        {
            FSet = fSet;
        }

        public FSet<T> FSet { get; }

        protected abstract string[] DisplayInfos { get; }

        public bool Equals(Elt<T> other) => HashCode == other.HashCode;
        public int CompareTo(Elt<T> other)
        {
            for (int k = 0; k < table.Length; ++k)
            {
                var e0 = table[k];
                var e1 = other.table[k];
                if (!e0.Equals(e1))
                    return e0.CompareTo(e1);
            }

            return 0;
        }

        public void CopyTo(T[] cache) => table.ReCopyTo(cache);

        public void Display(string name = "")
        {
            var nm = "s";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }

        public override string ToString() => string.Format(FSet.FmtElt, DisplayInfos);

        public override int GetHashCode() => base.GetHashCode();
    }

    public class EltComparer<T, U> : Comparer<U> where U : Elt<T> where T : struct, IEquatable<T>, IComparable<T>
    {
        public override int Compare(U x, U y) => x.CompareTo(y);
    }

    public class ObjEquality<T> : EqualityComparer<T> where T : AObj
    {
        public override bool Equals(T x, T y) => x.HashCode.Equals(y.HashCode);

        public override int GetHashCode(T obj) => obj.HashCode;
    }

    public class ComparerHash : EqualityComparer<HashSet<int>>
    {
        public override bool Equals(HashSet<int> x, HashSet<int> y) => x.SetEquals(y);

        public override int GetHashCode(HashSet<int> obj) => 1;
    }

    public abstract class Monoid<T> : FSet<T> where T : struct
    {
        readonly Dictionary<(int, int), int> tableOp = new Dictionary<(int, int), int>();
        private int IdHash;

        protected Monoid(int hash) : base(hash) { }

        protected void MonoidOpAdd(int h0, int h1, int h2)
        {
            tableOp[(h0, h1)] = h2;

            if (h0 == h2 && h0 == h1)
                IdHash = h0;

            if (h2 == IdHash)
            {
                tableOp[(h0, -1)] = h1;
                tableOp[(h1, -1)] = h0;
            }
        }

        public bool MonoidContains(int h0, int h1) => tableOp.ContainsKey((h0, h1));
        public bool HasInvert(int h) => tableOp.ContainsKey((h, -1));
        public int GetInvert(int h) => tableOp[(h, -1)];
        public int MonoidOp(int h0, int h1) => tableOp[(h0, h1)];

        public override int GetHashCode() => base.GetHashCode();
    }

}
