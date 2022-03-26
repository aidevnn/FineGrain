using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public static class Helpers
    {
        public static void ReCopyTo<T>(this T[] arr0, T[] arr1, int start0 = 0, int start1 = 0)
        {
            var step = Math.Min(arr0.Length - start0, arr1.Length - start1);
            for (int k = 0; k < step; ++k)
                arr1[k + start1] = arr0[k + start0];

        }

        public static byte[] SnIdentity(int n) => Enumerable.Range(0, n).Select(a => (byte)a).ToArray();
        public static int SnIdHash(int n) => GenHash(n, SnIdentity(n));

        public static int GenHash(int[] dims)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < dims.Length; ++k)
            {
                hash += pow * dims[k];
                pow *= dims[k];
            }

            return hash;
        }

        public static int GenHash<T>(int[] n, T[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k].GetHashCode();
                pow *= n[k];
            }

            return hash;
        }

        public static int GenHash<T>(int[] n, T m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < n.Length; ++k)
            {
                hash += pow * m.GetHashCode();
                pow *= n[k];
            }

            return hash;
        }

        public static int GenHash<T>(int n, T[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k].GetHashCode();
                pow *= n;
            }

            return hash;
        }

        public static bool IsZero(int[] a) => a.All(e => e == 0);

        public static bool IsIdentity(byte[] arr)
        {
            for (byte k = 0; k < arr.Length; ++k)
            {
                if (k != arr[k])
                    return false;
            }

            return true;
        }

        public static void ComposePermutation(byte[] arr0, byte[] arr1, byte[] arr2)
        {
            for (byte k = 0; k < arr2.Length; ++k)
                arr2[k] = arr0[arr1[k]];
        }

        public static bool CheckArray(int n, byte[] cycle)
        {
            if (cycle.Min() < 1 || cycle.Max() > n)
                return false;

            if (cycle.Distinct().Count() != cycle.Length)
                return false;

            return true;
        }

        public static void ComposeCycle(byte[] arr0, byte[] cycle)
        {
            var c = arr0[cycle[0]];
            for (int k = 0; k < cycle.Length - 1; ++k)
                arr0[cycle[k]] = arr0[cycle[k + 1]];

            arr0[cycle[cycle.Length - 1]] = c;
        }

        //public static int[][] DecomposeCycle(int[] arr0)
        //{
        //    List<List<int>> all = new List<List<int>>();
        //    HashSet<int> seq = new HashSet<int>(Enumerable.Range(0, arr0.Length));
        //    while (seq.Count != 0)
        //    {
        //        var t0 = seq.First();
        //        seq.Remove(t0);
        //        var tmp = new List<int>();
        //        var t1 = t0;
        //        while(true)
        //        {
        //            t0 = t1;
        //            tmp.Add(t0);
        //            t1 = arr0[t0];
        //            if (t0 != t1) seq.Remove(t1);
        //            else break;
        //        }
        //        all.Add(tmp);
        //    }

        //    return all.Select(a => a.ToArray()).ToArray();
        //}

        public static int ComputeOrder(byte[] arr0, byte[] arr1, byte[] arr2)
        {
            int order = 0;
            while (true)
            {
                ++order;
                ComposePermutation(arr0, arr1, arr2);
                if (!IsIdentity(arr1))
                    arr2.CopyTo(arr1, 0);
                else
                    break;
            }

            return order;
        }

        public static int ComputeSgn(byte[] arr)
        {
            int sgn = 1;
            for (int i = 1; i < arr.Length - 1; ++i)
                for (int j = i + 1; j < arr.Length; ++j)
                    if (arr[i] > arr[j])
                        sgn *= -1;

            return sgn;
        }

        public static bool Convexity(byte[] arr)
        {
            bool next = true, prev = true;
            int n = arr.Length - 1;
            for (int k0 = 1; k0 <= n; ++k0)
            {
                var k1 = (k0 == n ? 1 : k0 + 1);
                var e0 = arr[k0];
                var e1 = arr[k1];
                next &= e1 == (e0 == n ? 1 : e0 + 1);
                prev &= e1 == (e0 == 1 ? n : e0 - 1);
            }

            return next || prev;
        }

        public static void AddMod(byte[] n, byte[] m0, byte[] m1, byte[] m2)
        {
            for (int k = 0; k < m0.Length; ++k)
                m2[k] = (byte)((m0[k] + m1[k]) % n[k]);
        }

        public static byte[][] AllPermutation(int order, bool addZero = true)
        {
            var pool = Enumerable.Range(1, order).Select(a => (byte)a).ToList();
            var acc = new List<List<byte>>() { new List<byte>() };
            var tmpPool = new Queue<byte>(pool);
            while (tmpPool.Count != 0)
            {
                var p = tmpPool.Dequeue();
                var tmpAcc = new List<List<byte>>();
                foreach (var l0 in acc)
                {
                    for (int k = 0; k <= l0.Count; ++k)
                    {
                        var l1 = l0.ToList();
                        l1.Insert(k, p);
                        tmpAcc.Add(l1);
                    }
                }

                acc = tmpAcc;
            }

            if (addZero)
            {
                foreach (var e in acc)
                    e.Insert(0, 0);
            }

            return acc.Select(a => a.ToArray()).ToArray();
        }

        public static byte[] Canonic(int dim, int rank)
        {
            byte[] table = new byte[dim];
            table[rank] = 1;
            return table;
        }

        public static byte[][] AllTuples(params int[] dims)
        {
            var acc = new List<List<byte>>() { new List<byte>() };
            for (int i = 0; i < dims.Length; ++i)
            {
                var tmpAcc = new List<List<byte>>();
                foreach (var l0 in acc)
                {
                    for (byte k = 0; k < dims[i]; ++k)
                    {
                        var l1 = l0.ToList();
                        l1.Add(k);
                        tmpAcc.Add(l1);
                    }
                }

                acc = tmpAcc.ToList();
            }

            return acc.Select(a => a.ToArray()).ToArray();
        }

        public static byte[][] AllCombinaisons(int n)
        {
            var acc = new List<List<byte>>() { new List<byte>() };
            for (byte i = 0; i < n; ++i)
            {
                var tmpAcc = acc.ToList();
                foreach(var l0 in tmpAcc)
                {
                    var l1 = l0.ToList();
                    l1.Add(i);
                    acc.Add(l1);
                }
            }

            return acc.Select(a => a.ToArray()).ToArray();
        }
    }
}