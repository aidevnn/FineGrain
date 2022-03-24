using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class Integer256 : GElt<byte>
    {
        public Group256 Group { get; }
        public Integer256(Group256 group) : base(group, 0)
        {
            Group = group;
            table = new byte[1];
            V = 0;
        }

        public Integer256(Group256 group, byte v) : base(group, v)
        {
            Group = group;
            table = new byte[] { v };
            V = v;
        }

        public byte V { get; }

        string OrderStr => $"{Order,3}";
        string TableStr => string.Join(" ", table.Select(e => $"{e,3}"));
        protected override string[] DisplayInfos => new string[] { TableStr, OrderStr };
    }

    public class Group256 : FGroup<byte, Integer256>
    {
        public Group256() : base(257)
        {
            Fmt = "|G| = {0} in Z256";
            FmtElt = "({0})[{1}]";

            CreateCaches(1);
            CreateIdentity(new Integer256(this, 0));
            CreateElement(1);
        }

        protected override Integer256 Create(params byte[] ts)
        {
            return new Integer256(this, ts[0]);
        }

        protected override Integer256 DefineOp(Integer256 a, Integer256 b)
        {
            return new Integer256(this, (byte)(a.V + b.V));
        }
    }
}
