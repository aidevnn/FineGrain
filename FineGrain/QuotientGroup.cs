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
            Console.WriteLine("Class of : {0}", Representant);
        }

        public void DisplayEquivalents()
        {
            foreach (var e in elements)
                Console.WriteLine("\t{0}", e);
        }

        public void Display()
        {
            DisplayHead();
            Console.WriteLine("    Represent");
            DisplayEquivalents();
        }
    }

    public class QuotientGroup<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public GroupSubSet<T, U> G { get; private set; }
        public GroupSubSet<T, U> H { get; private set; }
        public GroupSubSet<T, U> G_over_H { get; private set; }
        public GroupOpLR OpLR { get; private set; }

        readonly Dictionary<int, int> representatives = new Dictionary<int, int>();
        readonly Dictionary<U, ClassModulo<T, U>> classOf = new Dictionary<U, ClassModulo<T, U>>();
        public Dictionary<U, ClassModulo<T, U>> ClassOf => new Dictionary<U, ClassModulo<T, U>>(classOf);
        public QuotientGroup(GroupSubSet<T, U> upGroup, GroupSubSet<T, U> subGroup, GroupOpLR opLR = GroupOpLR.Both) : base(upGroup.FGroup)
        {
            G = upGroup;
            H = subGroup;
            OpLR = opLR;
            SortBy = SortBy.Value;

            Name = $"{G.Name}/{H.Name}";
            Init();
        }

        void Init()
        {
            Fmt = string.Format("|{{0}}| = {{1}} with |{0}| = {2} and |{1}| = {3}, Op{4}", G.Name, H.Name, G.Count, H.Count, OpLR);

            var gIsGr = G.IsGroup();
            var hIsGr = H.IsGroup();

            if (G.FGroup.HashCode != H.FGroup.HashCode)
                return;

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

            Classes();
        }

        bool OpCompatible()
        {
            var lt = G.Elements;
            List<(U, U)> lt0 = new List<(U, U)>();
            foreach (var e0 in lt)
            {
                foreach (var e1 in lt)
                {
                    var e2 = FGroup.Op(e0, FGroup.Invert(e1));
                    if (H.Contains(e2))
                        lt0.Add((e0, e1));
                }
            }

            foreach(var (e0, e1) in lt0)
            {
                foreach (var a in lt)
                {
                    U e0a, e1a, e2a;
                    if (OpLR == GroupOpLR.Left || OpLR == GroupOpLR.Both)
                    {
                        e0a = FGroup.Op(a, e0);
                        e1a = FGroup.Op(a, e1);
                        e2a = FGroup.Op(e0a, FGroup.Invert(e1a));
                        if (!H.Contains(e2a))
                            return false;
                    }

                    if (OpLR == GroupOpLR.Right || OpLR == GroupOpLR.Both)
                    {
                        e0a = FGroup.Op(e0, a);
                        e1a = FGroup.Op(e1, a);
                        e2a = FGroup.Op(e0a, FGroup.Invert(e1a));
                        if (!H.Contains(e2a))
                            return false;
                    }

                }
            }

            return true;
        }

        void Classes()
        {
            var comp = OpCompatible();
            if (!comp)
                return;

            elements.Clear();
            var lt = new SortedSet<U>(G.Elements, GetComparer);
            var hs0 = new HashSet<HashSet<int>>(new ComparerHash());
            var hs1 = new HashSet<HashSet<int>>(new ComparerHash());
            foreach (var e0 in lt)
            {
                if (OpLR == GroupOpLR.Left || OpLR == GroupOpLR.Both)
                {
                    var equiv = new GroupOp<T, U>(e0, H);
                    hs0.Add(equiv.Elements.Select(a => a.HashCode).ToHashSet());
                }

                if (OpLR == GroupOpLR.Right || OpLR == GroupOpLR.Both)
                {
                    var equiv = new GroupOp<T, U>(H, e0);
                    hs1.Add(equiv.Elements.Select(a => a.HashCode).ToHashSet());
                }
            }

            if (OpLR == GroupOpLR.Right)
                hs0 = new HashSet<HashSet<int>>(hs1);

            if (OpLR == GroupOpLR.Both)
            {
                if (hs0.Count != hs1.Count || hs0.Any(lh => !hs1.Contains(lh)))
                    return;
            }

            foreach (var lh in hs0)
            {
                var lu = new SortedSet<U>(lh.Select(FGroup.GetElement<U>), GetComparer);
                var r = lu.First();
                var subG = new GroupSubSet<T, U>(FGroup, lu.ToArray());
                classOf[r] = new ClassModulo<T, U>(r, subG);
                elements.Add(r);
                foreach (var e in lu)
                    representatives[e.HashCode] = r.HashCode;
            }
        }

        public void DisplayClasses()
        {
            foreach (var e in classOf)
                e.Value.Display();

            Console.WriteLine();
        }

        public override U Invert(U e)
        {
            var h0 = representatives[e.HashCode];
            var h1 = FGroup.GetInvert(h0);
            var r = representatives[h1];
            return FGroup.GetElement<U>(r);
        }

        public override U Op(U a, U b)
        {
            var ha = representatives[a.HashCode];
            var hb = representatives[b.HashCode];
            var hc = FGroup.MonoidOp(a.HashCode, b.HashCode);
            var r = representatives[hc];
            return FGroup.GetElement<U>(r);
        }
    }
}
