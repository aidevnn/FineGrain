using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class GroupSubSet<T, U> : SubFSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public FGroup<T, U> FGroup { get; }
        public override Comparer<U> GetComparer => SortBy == SortBy.Order ? new GEltComparer<T, U>() : (Comparer<U>)new EltComparer<T, U>();
        public GroupSubSet(FGroup<T, U> fGroup) : base(fGroup)
        {
            FGroup = fGroup;
            elements = new SortedSet<U>();
            SortBy = SortBy.Order;
        }

        public GroupSubSet(FGroup<T, U> fGroup, string name) : this(fGroup)
        {
            FGroup = fGroup;
            Name = name;
        }

        public GroupSubSet(FGroup<T, U> fGroup, string name, string fmt) : this(fGroup, name)
        {
            Fmt = fmt;
        }

        public GroupSubSet(FGroup<T, U> fGroup, IEnumerable<U> us) : base(fGroup)
        {
            if (us.Any(e => !FSet.HashCode.Equals(e.FSet.HashCode)))
                return;

            FGroup = fGroup;
            var comparer = SortBy == SortBy.Order ? new GEltComparer<T, U>() : (Comparer<U>)new EltComparer<T, U>();
            elements = new SortedSet<U>();
            AddRange(us, true);
            SortBy = SortBy.Order;
        }

        public GroupSubSet(FGroup<T, U> fGroup, IEnumerable<U> us, string name) : this(fGroup, us)
        {
            Name = name;
        }

        public GroupSubSet(FGroup<T, U> fGroup, IEnumerable<U> us, string name, string fmt) : this(fGroup, us, name)
        {
            Fmt = fmt;
        }

        public virtual U Invert(U e) => FGroup.Invert(e);
        public virtual U Op(U a, U b) => FGroup.Op(a, b);

        public bool IsGroup()
        {
            foreach (var e0 in elements)
            {
                if (!Contains(Invert(e0)))
                    return false;

                foreach (var e1 in elements)
                {
                    if (!Contains(Op(e0, e1)))
                        return false;
                }
            }

            return true;
        }

        public bool IsCommutative()
        {
            foreach (var e0 in elements)
            {
                foreach (var e1 in elements)
                {
                    var e20 = Op(e0, e1);
                    var e21 = Op(e1, e0);
                    if (e20.HashCode != e21.HashCode)
                        return false;
                }
            }

            return true;
        }

        public void Details()
        {
            DisplayElements();
            Table();
            Console.WriteLine();
        }

        public override void DisplayHead()
        {
            base.DisplayHead();
            var isGr = IsGroup();
            Console.WriteLine("IsGroup      :{0,5}", isGr);
            Console.WriteLine("IsCommutative:{0,5}", IsCommutative());
            Console.WriteLine();
        }

        public void DisplayElements() => base.DisplayElements(!IsGroup()); 

        public void Table()
        {
            if (elements.Count == 0)
            {
                Console.WriteLine("Empty Set");
                return;
            }

            var elts = Elements;
            elements = new SortedSet<U>(elts, GetComparer);

            var isGr = IsGroup();
            base.DisplayHead();
            if (!isGr)
            {
                Console.WriteLine("Not a Group, need to be amplified");
                return;
            }

            if (elements.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(elements.Count, !isGr).Select(w => w[0]).ToList();
            Dictionary<char, U> ce = new Dictionary<char, U>();
            Dictionary<U, char> ec = new Dictionary<U, char>(new ObjEquality<U>());

            for (int k = 0; k < elements.Count; ++k)
            {
                var c = word[k];
                var e = elements.ElementAt(k);
                ce[c] = e;
                ec[e] = c;
            }

            string MyFormat(string c, string g, List<char> l) => string.Format("{0,2}|{1}", c, string.Join(g, l));

            var head = MyFormat("*", " ", word);
            var line = MyFormat("--", "", Enumerable.Repeat('-', word.Count * 2).ToList());
            Console.WriteLine(head);
            Console.WriteLine(line);

            foreach (var e0 in elements)
            {
                var v0 = ec[e0].ToString();
                var l0 = elements.Select(e1 => ec[Op(e1, e0)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }
    }

    public enum GroupOpLR { Left, Right, Both }
    public class GroupOp<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public GroupOpLR OpLR { get; }
        public GroupSubSet<T, U> SubGroup { get; }
        public GroupOp(FGroup<T, U> fGroup, U e) : base(fGroup, new U[] { e }) { }
        public GroupOp(GroupSubSet<T, U> subGroup, U e) : base(subGroup.FGroup)
        {
            if (!subGroup.FSet.Equals(e.FSet))
                return;

            SubGroup = subGroup;
            AddRange(subGroup.Elements.Select(e0 => FGroup.Op(e0, e)));
            OpLR = GroupOpLR.Right;
        }

        public GroupOp(U e, GroupSubSet<T, U> subGroup) : base(subGroup.FGroup)
        {
            if (!subGroup.FSet.Equals(e.FSet))
                return;

            AddRange(subGroup.Elements.Select(e0 => FGroup.Op(e, e0)));
            OpLR = GroupOpLR.Left;
        }

        public GroupOp(GroupSubSet<T, U> subGroup, U e, string name) : this(subGroup, e)
        {
            Name = name;
        }

        public GroupOp(GroupSubSet<T, U> subGroup, U e, string name, string fmt) : this(subGroup, e, name)
        {
            Fmt = fmt;
        }

        public GroupOp(U e, GroupSubSet<T, U> subGroup, string name) : this(e, subGroup)
        {
            Name = name;
        }

        public GroupOp(U e, GroupSubSet<T, U> subGroup, string name, string fmt) : this(e, subGroup, name)
        {
            Fmt = fmt;
        }
    }

    public class Monogenic<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public Monogenic(FGroup<T, U> fGroup, U e) : base(fGroup)
        {
            if (!e.FSet.Equals(fGroup))
                return;

            List<U> elts = new List<U>() { fGroup.Identity };
            for (int k = 0; k < e.Order; k++)
            {
                var e0 = elts.Last();
                elts.Add(FGroup.Op(e, e0));
            }

            AddRange(elts);
        }

        public Monogenic(FGroup<T, U> fGroup, U e, string name) : this(fGroup, e)
        {
            Name = name;
        }

        public Monogenic(FGroup<T, U> fGroup, U e, string name, string fmt) : this(fGroup, e)
        {
            Name = name;
            Fmt = fmt;
        }
    }

    public class GeneratedSubGroup<T,U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public GeneratedSubGroup(FGroup<T, U> fGroup, params U[] us) : base(fGroup, us)
        {
            Amplify();
        }

        public GeneratedSubGroup(FGroup<T, U> fGroup, string name, params U[] us) : this(fGroup, us)
        {
            Name = name;
        }

        public GeneratedSubGroup(FGroup<T, U> fGroup, string name, string fmt, params U[] us) : this(fGroup, name, us)
        {
            Fmt = fmt;
        }

        void Amplify()
        {
            var hs = new HashSet<U>(elements);

            int sz = 0;
            while (sz != hs.Count)
            {
                sz = hs.Count;
                var lt = hs.ToList();
                foreach (var e0 in lt)
                {
                    foreach (var e1 in lt)
                    {
                        var e2 = FGroup.Op(e0, e1);
                        hs.Add(e2);
                    }
                }
            }

            elements.Clear();
            foreach (var e in hs)
                elements.Add(e);
        }

    }
}
