using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class Permutation : GElt<byte>
    {
        public Sigma Sn { get; }
        public int Sgn { get; }
        public Permutation(Sigma sigma, byte[] arr, int hash) : base(sigma, hash)
        {
            Sn = sigma;
            arr.ReCopyTo(table);
            Sgn = Helpers.ComputeSgn(table);
        }

        public Permutation(FSet<byte> fSet, GElt<byte> e) : base(fSet, e)
        {
            Sgn = Helpers.ComputeSgn(table);
        }

        string SgnStr => Sgn == 1 ? "+" : "-";
        string OrderStr => $"{Order,2}{SgnStr}";
        string TableStr => string.Join(" ", table.Skip(1).Select(e => $"{e,2}"));
        protected override string[] DisplayInfos => new string[] { TableStr, OrderStr };

    }

    public class Sigma : FGroup<byte, Permutation>
    {
        public int N { get; }
        public Sigma(int n) : base(n)
        {
            N = n;
            CreateCaches(N + 1);
            SetIdentity();
        }

        protected override Permutation Create(params byte[] ts)
        {
            var hash = Helpers.GenHash(N + 1, ts);
            return new Permutation(this, ts, hash);
        }

        protected override Permutation CreateIdentity()
        {
            var arr = Enumerable.Range(0, N + 1).Select(a => (byte)a).ToArray();
            var hash = Helpers.GenHash(N + 1, arr);
            return new Permutation(this, arr, hash);
        }

        public override Permutation Clone(FSet<byte> fSet, Permutation e) => new Permutation(fSet, e);

        protected override Permutation DefineOp(Permutation a, Permutation b)
        {
            ClearCaches();
            a.CopyTo(cache0);
            b.CopyTo(cache1);
            Helpers.ComposePermutation(cache0, cache1, cache2);
            var hash = Helpers.GenHash(N + 1, cache2);
            return new Permutation(this, cache2, hash);
        }

        public Permutation Table(params byte[] vs)
        {
            if (!Helpers.CheckArray(N, vs))
                return Identity;

            ClearCaches();
            vs.ReCopyTo(cache0, 0, 1);
            return CreateElement(cache0);
        }

        public Permutation Cycle(params byte[] cycle)
        {
            ClearCaches();
            Identity.CopyTo(cache2);
            Helpers.ComposeCycle(cache2, cycle);
            return CreateElement(cache2);
        }

        public Permutation Cycle(SingleTuple<byte> cycle) => Cycle(cycle.Table);
        public Permutation Cycles(params SingleTuple<byte>[] cycles)
        {
            ClearCaches();
            Identity.CopyTo(cache2);
            foreach (var c in cycles)
                Helpers.ComposeCycle(cache2, c.Table);

            return CreateElement(cache2);
        }

        public void GenerateAll()
        {
            var perms = Helpers.AllPermutation(N);
            foreach (var p in perms)
                CreateElement(p);
        }

        public Permutation kCycle(int count) => Cycle(KCycle(count));
        public Permutation kCycleR(int count) => Cycle(KCycleR(count));
        public Permutation kCycle(int start, int count) => Cycle(KCycle(start, count));
        public Permutation kCycleR(int start, int count) => Cycle(KCycleR(start, count));

        public static SingleTuple<byte> KCycle(int start, int count) => new SingleTuple<byte>(Enumerable.Range(start, count).Select(a => (byte)a).ToArray());
        public static SingleTuple<byte> KCycleR(int start, int count) => new SingleTuple<byte>(Enumerable.Range(start, count).Reverse().Select(a => (byte)a).ToArray());
        public static SingleTuple<byte> KCycle(int count) => KCycle(1, count);
        public static SingleTuple<byte> KCycleR(int count) => KCycleR(1, count);
    }
}
