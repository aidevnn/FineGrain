using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class Tuple256 : GElt<byte>
    {
        public Abelian256 Gr { get; }
        public Tuple256(Abelian256 gr) : base(gr, 0)
        {
            table = new byte[gr.CacheLength];
            Gr = gr;
        }

        public Tuple256(Abelian256 gr, byte[] arr, int hash) : base(gr, hash)
        {
            table = arr.ToArray();
            Gr = gr;
        }

        string OrderStr => $"{Order,3}";
        string TableStr => string.Join(" ", table.Select(e => $"{e,3}"));
        protected override string[] DisplayInfos => new string[] { TableStr, OrderStr };
    }

    public class Abelian256 : FGroup<byte, Tuple256>
    {
        public int[] Dim { get; }
        public Abelian256(params int[] dim) : base(dim)
        {
            Dim = dim;
            SetIdentity();
        }

        protected override Tuple256 Create(params byte[] ts)
        {
            ClearCaches();
            ts.ReCopyTo(cache1);
            Helpers.AddMod(Dim, cache0, cache1, cache2);
            var hash = Helpers.GenHash(Dim, cache2);
            var p = new Tuple256(this, cache2, hash);
            return p;
        }

        protected override Tuple256 CreateIdentity() => new Tuple256(this);

        protected override Tuple256 DefineOp(Tuple256 a, Tuple256 b)
        {
            ClearCaches();
            a.CopyTo(cache0);
            b.CopyTo(cache1);
            Helpers.AddMod(Dim, cache0, cache1, cache2);
            var hash = Helpers.GenHash(Dim, cache2);
            var p = new Tuple256(this, cache2, hash);
            return p;
        }

        Tuple256 CreateFrom(SingleTuple<byte> tuple) => CreateElement(tuple.Table);

        public GroupSubSet<byte, Tuple256> Generate(params byte[] vs)
        {
            return new GeneratedSubGroup<byte, Tuple256>(this, CreateElement(vs));
        }

        public GroupSubSet<byte, Tuple256> Generate(string name, params byte[] vs)
        {
            return new GeneratedSubGroup<byte, Tuple256>(this, name, CreateElement(vs));
        }

        public GroupSubSet<byte, Tuple256> Generate(params SingleTuple<byte>[] vs)
        {
            var us = vs.Select(CreateFrom).ToArray();
            return new GeneratedSubGroup<byte, Tuple256>(this, us);
        }

        public GroupSubSet<byte, Tuple256> Generate(string name, params SingleTuple<byte>[] vs)
        {
            return new GeneratedSubGroup<byte, Tuple256>(this, name, vs.Select(CreateFrom).ToArray());
        }

    }
}
