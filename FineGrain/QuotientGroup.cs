using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class ClassModulo<T, U> : SubFSet<T, U>
    where T : struct, IComparable<T>, IEquatable<T>
    where U : Elt<T>, IComparable<Elt<T>>
    {
        public U Representant { get; }
        public ClassModulo(U representant, SubFSet<T, U> subFSet) : base(subFSet.FSet)
        {
            Representant = representant;
            elements = new SortedSet<U>(subFSet.Elements, new EltComparer<T, U>());
        }

        public override void DisplayHead()
        {
            Console.WriteLine("Class of : {0} Modulo H", Representant);
        }

        public void DisplayEquivalents()
        {
            foreach (var e in elements)
                Console.WriteLine("\t{0}", e);
        }
    }

    public class GroupQuotient<T, U> : FGroup<T, U>
    where T : struct, IComparable<T>, IEquatable<T>
    where U : GElt<T>, IComparable<Elt<T>>
    {
        public GroupSubSet<T, U> G { get; private set; }
        public GroupSubSet<T, U> H { get; private set; }
        public GroupSubSet<T, U> G_over_H { get; private set; }
        public FGroup<T, U> FGroup { get; private set; }
        public GroupOpLR OpLR { get; private set; }

        readonly Dictionary<U, ClassModulo<T, U>> classOf = new Dictionary<U, ClassModulo<T, U>>();
        readonly Dictionary<U, U> representatives = new Dictionary<U, U>();
        public GroupQuotient(GroupSubSet<T, U> upGroup, GroupSubSet<T, U> subGroup, GroupOpLR opLR) : base(upGroup.FGroup.HashCode)
        {
            FGroup = upGroup.FGroup;

            G = upGroup;
            H = subGroup;

            var gIsGr = G.IsGroup();
            var hIsGr = H.IsGroup();

            if (!gIsGr || !hIsGr)
                return;

            foreach (var e in H.Elements)
            {
                if (!G.Contains(e))
                {
                    Console.WriteLine(e);
                    return;
                }
            }

            var gr = $"|G|={G.Elements.Count} and |H|={H.Elements.Count}";
            Fmt = "|G/H| = {0}, " + gr;
            FmtElt = "({1})[{0}]";
            CreateCaches(FGroup.CacheLength);
            CreateIdentity(FGroup.Identity);
            CreateClasses();
        }

        void CreateClasses()
        {
            var lt = new SortedSet<U>(G.Elements, new EltComparer<T, U>());
            foreach (var e0 in lt)
            {
                if (classOf.Any(mod => mod.Value.Contains(e0)))
                    continue;

                if (OpLR == GroupOpLR.Left)
                    classOf[e0] = new ClassModulo<T, U>(e0, new GroupOp<T, U>(e0, H));
                else
                    classOf[e0] = new ClassModulo<T, U>(e0, new GroupOp<T, U>(H, e0));
            }

            foreach (var kp in classOf)
            {
                foreach (var e0 in kp.Value.Elements)
                    representatives[e0] = kp.Key;
            }

            foreach (var e0 in lt)
            {
                foreach (var e1 in lt)
                {
                    Op(e0, e1);
                }
            }

            G_over_H = new GroupSubSet<T, U>(this, Elements());
        }

        protected override U DefineOp(U a, U b)
        {
            var ra = representatives[a];
            var rb = representatives[b];

            var e = FGroup.Op(ra, rb);

            var re = representatives[e];
            TableOpAdd(ra.HashCode, rb.HashCode, re.HashCode);
            return re;
        }

        public GroupSubSet<T, U> SubGroup => G_over_H;
        public void Details() => G_over_H.Details();

        protected override U Create(params T[] ts) => Identity;
    }
}
