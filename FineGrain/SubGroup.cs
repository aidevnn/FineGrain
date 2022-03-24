using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class GroupSubSet<T, U> : SubFSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public FGroup<T, U> FGroup { get; }
        public GroupSubSet(FGroup<T, U> fGroup) : base(fGroup)
        {
            FGroup = fGroup;
        }

        public GroupSubSet(FGroup<T, U> fGroup, IEnumerable<U> us) : base(fGroup)
        {
            if (us.Any(e => !FSet.HashCode.Equals(e.FSet.HashCode)))
                return;

            FGroup = fGroup;
            AddRange(us, true);
        }

        public bool IsGroup()
        {
            foreach (var e0 in elements)
            {
                if (!Contains(FGroup.Invert(e0)))
                    return false;

                foreach (var e1 in elements)
                {
                    if (!Contains(FGroup.Op(e0, e1)))
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
                    var e20 = FGroup.Op(e0, e1);
                    var e21 = FGroup.Op(e1, e0);
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
        }

        public void DisplayElements() => base.DisplayElements(!IsGroup());

        public void Table()
        {
            if (elements.Count == 0)
            {
                Console.WriteLine("Empty Set");
                return;
            }

            var isGr = IsGroup();
            base.DisplayHead();
            if (!isGr)
            {
                Console.WriteLine("Not a Group, need to be amplified");
                return;
            }

            if (Elements.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(Elements.Count, !isGr).Select(w => w[0]).ToList();
            Dictionary<char, U> ce = new Dictionary<char, U>();
            Dictionary<U, char> ec = new Dictionary<U, char>(new ObjEquality<U>());

            for (int k = 0; k < Elements.Count; ++k)
            {
                var c = word[k];
                var e = Elements.ElementAt(k);
                ce[c] = e;
                ec[e] = c;
            }

            string MyFormat(string c, string g, List<char> l) => string.Format("{0,2}|{1}", c, string.Join(g, l));

            var head = MyFormat("*", " ", word);
            var line = MyFormat("--", "", Enumerable.Repeat('-', word.Count * 2).ToList());
            Console.WriteLine(head);
            Console.WriteLine(line);

            foreach (var e0 in Elements)
            {
                var v0 = ec[e0].ToString();
                var l0 = Elements.Select(e1 => ec[FGroup.Op(e1, e0)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }
    }

    public enum GroupOpLR { Left, Right }
    public class GroupOp<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public GroupOpLR OpLR { get; }
        public GroupOp(FGroup<T, U> fGroup, U e) : base(fGroup, new U[] { e }) { }
        public GroupOp(GroupSubSet<T, U> subSet, U e) : base(subSet.FGroup)
        {
            if (!subSet.FSet.Equals(e.FSet))
                return;

            AddRange(subSet.Elements.Select(e0 => FGroup.Op(e0, e)));
            OpLR = GroupOpLR.Right;
        }

        public GroupOp(U e, GroupSubSet<T, U> subSet) : base(subSet.FGroup)
        {
            if (!subSet.FSet.Equals(e.FSet))
                return;

            AddRange(subSet.Elements.Select(e0 => FGroup.Op(e, e0)));
            OpLR = GroupOpLR.Left;
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
    }
}
