using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class Integer256 : GElt<byte>
    {
        public Group256 Group { get; }
        public Integer256(FSet<byte> fSet, Integer256 integer) : base(fSet, integer) { }

        public Integer256(Group256 group, byte v) : base(group, (v % group.N))
        {
            Group = group;
            byte v0 = (byte)HashCode;
            table = new byte[] { v0 };
            V = v0;
        }

        public byte V { get; }

        string OrderStr => $"{Order,3}";
        string TableStr => string.Join(" ", table.Select(e => $"{e,3}"));
        protected override string[] DisplayInfos => new string[] { TableStr, OrderStr };
    }

    public class Group256 : FGroup<byte, Integer256>
    {
        public int N { get; }
        public Group256(byte n = 128) : base(257)
        {
            N = n;
            SetIdentity();
        }

        protected override Integer256 CreateIdentity() => Create(0);

        protected override Integer256 Create(params byte[] ts) => new Integer256(this, ts[0]);

        public override Integer256 Clone(FSet<byte> fSet, Integer256 e) => new Integer256(fSet, e);

        protected override Integer256 DefineOp(Integer256 a, Integer256 b) => new Integer256(this, (byte)(((int)a.V + (int)b.V) % N));

        public void GenerateAll() => CreateElement(1);

        public Integer256 ByValue(byte b) => CreateElement(b);

        public GroupSubSet<byte, Integer256> Generate(params Integer256[] vs)
        {
            return new GeneratedSubGroup<byte, Integer256>(this, vs);
        }

        public GroupSubSet<byte, Integer256> Generate(string name, params Integer256[] vs)
        {
            return new GeneratedSubGroup<byte, Integer256>(this, name, vs);
        }

        public GroupSubSet<byte, Integer256> Generate(params byte[] vs)
        {
            return new GeneratedSubGroup<byte, Integer256>(this, vs.Select(ByValue).ToArray());
        }

        public GroupSubSet<byte, Integer256> Generate(string name, params byte[] vs)
        {
            return new GeneratedSubGroup<byte, Integer256>(this, name, vs.Select(ByValue).ToArray());
        }

    }
}
