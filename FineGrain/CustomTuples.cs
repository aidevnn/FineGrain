using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public class SingleTuple<T> where T : struct
    {
        public T[] Table { get; private set; }
        public SingleTuple(params T[] table)
        {
            Table = table.ToArray();
        }

        public static implicit operator SingleTuple<T>(T a) => new SingleTuple<T>(a);
        public static implicit operator SingleTuple<T>((T a, T b) p) => new SingleTuple<T>(p.a, p.b);
        public static implicit operator SingleTuple<T>((T a, T b, T c) p) => new SingleTuple<T>(p.a, p.b, p.c);
        public static implicit operator SingleTuple<T>((T a, T b, T c, T d) p) => new SingleTuple<T>(p.a, p.b, p.c, p.d);
        public static implicit operator SingleTuple<T>((T a, T b, T c, T d, T e) p) => new SingleTuple<T>(p.a, p.b, p.c, p.d, p.e);
        public static implicit operator SingleTuple<T>((T a, T b, T c, T d, T e, T f) p) => new SingleTuple<T>(p.a, p.b, p.c, p.d, p.e, p.f);

        public override string ToString() => string.Format("({0})", string.Join(" ", Table.Select(e => $"{e,2}")));
    }

    public class ManyTuples<T> where T : struct
    {
        public SingleTuple<T>[] Tuples { get; private set; }
        public ManyTuples(params SingleTuple<T>[] tuples)
        {
            Tuples = tuples.ToArray();
        }

        public static implicit operator ManyTuples<T>(T a) => new ManyTuples<T>(a);
        public static implicit operator ManyTuples<T>((T a, T b) p) => new ManyTuples<T>(p);
        public static implicit operator ManyTuples<T>((T a, T b, T c) p) => new ManyTuples<T>(p);
        public static implicit operator ManyTuples<T>((T a, T b, T c, T d) p) => new ManyTuples<T>(p);
        public static implicit operator ManyTuples<T>((T a, T b, T c, T d, T e) p) => new ManyTuples<T>(p);
        public static implicit operator ManyTuples<T>((T a, T b, T c, T d, T e, T f) p) => new ManyTuples<T>(p);

        public static implicit operator ManyTuples<T>(SingleTuple<T> mc) => new ManyTuples<T>(mc);
        public static implicit operator ManyTuples<T>((SingleTuple<T> a, SingleTuple<T> b) mc) => new ManyTuples<T>(mc.a, mc.b);
        public static implicit operator ManyTuples<T>((SingleTuple<T> a, SingleTuple<T> b, SingleTuple<T> c) mc) => new ManyTuples<T>(mc.a, mc.b, mc.c);
        public static implicit operator ManyTuples<T>((SingleTuple<T> a, SingleTuple<T> b, SingleTuple<T> c, SingleTuple<T> d) mc) => new ManyTuples<T>(mc.a, mc.b, mc.c, mc.d);
        public static implicit operator ManyTuples<T>((SingleTuple<T> a, SingleTuple<T> b, SingleTuple<T> c, SingleTuple<T> d, SingleTuple<T> e) mc) => new ManyTuples<T>(mc.a, mc.b, mc.c, mc.d, mc.e);
        public static implicit operator ManyTuples<T>((SingleTuple<T> a, SingleTuple<T> b, SingleTuple<T> c, SingleTuple<T> d, SingleTuple<T> e, SingleTuple<T> f) mc) => new ManyTuples<T>(mc.a, mc.b, mc.c, mc.d, mc.e, mc.f);

        public override string ToString() => string.Format("[{0}]", string.Join(" ", Tuples.Select(e => $"{e}")));
    }
}
