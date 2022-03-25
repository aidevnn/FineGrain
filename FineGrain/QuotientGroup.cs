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
        readonly Dictionary<int, int> representatives = new Dictionary<int, int>();
        string Name { get; }
        public Dictionary<U, ClassModulo<T, U>> ClassOf => new Dictionary<U, ClassModulo<T, U>>(classOf);
        public GroupQuotient(FGroup<T, U> fGroup) : base(fGroup.HashCode)
        {
            FGroup = fGroup;
            G = new GroupSubSet<T, U>(fGroup, FGroup.Elements());
            H = new GroupSubSet<T, U>(fGroup, new U[] { fGroup.Identity }, "<@>");

            Name = "G/@";
            Init();
        }

        public GroupQuotient(FGroup<T, U> fGroup, string name) : base(fGroup.HashCode)
        {
            FGroup = fGroup;
            G = new GroupSubSet<T, U>(fGroup, FGroup.Elements());
            H = new GroupSubSet<T, U>(fGroup, new U[] { fGroup.Identity });

            Name = name;
            Init();
        }

        public GroupQuotient(GroupSubSet<T, U> upGroup, GroupSubSet<T, U> subGroup, GroupOpLR opLR) : base(upGroup.FGroup.HashCode)
        {
            FGroup = upGroup.FGroup;

            G = upGroup;
            H = subGroup;

            Name = $"{G.Name}/{H.Name}";
            Init();
        }

        public GroupQuotient(GroupSubSet<T, U> upGroup, GroupSubSet<T, U> subGroup, GroupOpLR opLR, string name) : base(upGroup.FGroup.HashCode)
        {
            FGroup = upGroup.FGroup;

            G = upGroup;
            H = subGroup;

            Name = name;
            Init();
        }

        void Init()
        {
            Fmt = string.Format("|{{0}}| = {{1}} with |{0}| = {2} and |{1}| = {3}", G.Name, H.Name, G.Count, H.Count);
            FmtElt = "({0})[{1}]";
            
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

            CreateCaches(FGroup.CacheLength);
            SetIdentity();
            CreateClasses();
        }

        void CreateClasses()
        {
            var lt = new SortedSet<U>(G.Elements, new EltComparer<T, U>());
            foreach (var e0 in lt)
            {
                if (representatives.ContainsKey(e0.HashCode))
                    continue;

                GroupSubSet<T, U> equivalents;
                if (OpLR == GroupOpLR.Left)
                    equivalents = new GroupOp<T, U>(e0, H);
                else
                    equivalents = new GroupOp<T, U>(H, e0);

                foreach (var e1 in equivalents.Elements)
                    representatives[e1.HashCode] = e0.HashCode;
            }

            var rep = representatives.Values.Distinct().ToList();
            foreach (var h0 in rep)
            {
                var e0 = FGroup.GetElement<U>(h0);
                foreach (var h1 in rep)
                {
                    var e1 = FGroup.GetElement<U>(h1);
                    Op(e0, e1);
                }
            }

            G_over_H = new GroupSubSet<T, U>(this, Elements(), Name);

            var allClasses = new Dictionary<int, List<int>>();
            foreach(var e in representatives)
            {
                if (!allClasses.ContainsKey(e.Value))
                    allClasses[e.Value] = new List<int>();

                allClasses[e.Value].Add(e.Key);
            }

            foreach (var kp in allClasses)
            {
                var r = FGroup.GetElement<U>(kp.Key);
                var subG = new GroupSubSet<T, U>(FGroup, kp.Value.Select(e => FGroup.GetElement<U>(e)).ToArray());
                classOf[r] = new ClassModulo<T, U>(r, subG);
            }
        }

        protected override U DefineOp(U a, U b)
        {
            var ra = representatives[a.HashCode];
            var rb = representatives[b.HashCode];

            var e = FGroup.MonoidOp(ra, rb);

            var re = representatives[e];
            var p = Clone(this, FGroup.GetElement<U>(re));
            return p;
        }

        public void Display() => G_over_H.DisplayElements();
        public void Details() => G_over_H.Details();

        protected override U Create(params T[] ts) => Clone(this, FGroup.CreateElement(ts));

        public override U Clone(FSet<T> fSet, U e) => FGroup.Clone(fSet, e);

        protected override U CreateIdentity()
        {
            foreach (var e in H.Elements)
                representatives[e.HashCode] = FGroup.Identity.HashCode;

            representatives[FGroup.Identity.HashCode] = FGroup.Identity.HashCode;
            return FGroup.Clone(this, FGroup.Identity);
        }

        public static implicit operator GroupSubSet<T, U>(GroupQuotient<T, U> qg) => qg.G_over_H;
    }
}
