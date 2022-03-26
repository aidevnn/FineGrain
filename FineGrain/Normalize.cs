using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class Normalize<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public Normalize(GroupSubSet<T, U> H, GroupSubSet<T, U> S) : base(H.FGroup)
        {
            if (S.FGroup.HashCode != H.FGroup.HashCode)
                return;

            var sElts = S.Elements;
            var hElts = H.Elements;
            foreach (var x in hElts)
            {
                if (sElts.All(s => S.Contains(xSx_1(s, x))))
                    elements.Add(x);
            }

            Name = "N";
        }

        public Normalize(GroupSubSet<T, U> H, params SingleTuple<T>[] us)
        : this(H, new GroupSubSet<T, U>(H.FGroup, us.Select(c => H.FGroup.CreateElement(c.Table)).ToArray()))
        {

        }

        U xSx_1(U s, U x) => FGroup.Op(FGroup.Op(x, s), FGroup.Invert(x));
    }

    public class Centerize<T, U> : GroupSubSet<T, U> where U : GElt<T>, IComparable<GElt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        public Centerize(GroupSubSet<T, U> H, GroupSubSet<T, U> S) : base(H.FGroup)
        {
            if (S.FGroup.HashCode != H.FGroup.HashCode)
                return;

            var sElts = S.Elements;
            var hElts = H.Elements;
            foreach (var x in hElts)
            {
                if (sElts.All(s => s.HashCode == (xSx_1(s, x)).HashCode))
                    elements.Add(x);
            }

            Name = "Z";
        }

        public Centerize(GroupSubSet<T, U> H, params SingleTuple<T>[] us)
        : this(H, new GroupSubSet<T, U>(H.FGroup, us.Select(c => H.FGroup.CreateElement(c.Table)).ToArray()))
        {

        }

        U xSx_1(U s, U x) => FGroup.Op(FGroup.Op(x, s), FGroup.Invert(x));
    }
}
