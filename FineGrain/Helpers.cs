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

        public static int[] SnIdentity(int n) => Enumerable.Range(0, n + 1).ToArray();
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

        public static int GenHash(int[] n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n[k];
            }

            return hash;
        }

        public static int GenHash(int[] n, int m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < n.Length; ++k)
            {
                hash += pow * m;
                pow *= n[k];
            }

            return hash;
        }

        public static int GenHash(int n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n;
            }

            return hash;
        }

        public static bool IsZero(int[] a) => a.All(e => e == 0);

        public static bool IsIdentity(int[] arr)
        {
            for (int k = 0; k < arr.Length; ++k)
            {
                if (k != arr[k])
                    return false;
            }

            return true;
        }

        public static void ComposePermutation(int[] arr0, int[] arr1, int[] arr2)
        {
            for (int k = 0; k < arr2.Length; ++k)
                arr2[k] = arr0[arr1[k]];
        }

        public static bool CheckArray(int n, int[] cycle)
        {
            if (cycle.Min() < 1 || cycle.Max() > n)
                return false;

            if (cycle.Distinct().Count() != cycle.Length)
                return false;

            return true;
        }

        public static void ComposeCycle(int[] arr0, int[] cycle)
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

        public static int ComputeOrder(int[] arr0, int[] arr1, int[] arr2)
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

        public static int ComputeSign(int[] arr)
        {
            int sgn = 1;
            for (int i = 1; i < arr.Length - 1; ++i)
                for (int j = i + 1; j < arr.Length; ++j)
                    if (arr[i] > arr[j])
                        sgn *= -1;

            return sgn;
        }

        public static bool Convexity(int[] arr)
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

        public static void AddMod(int[] n, int[] m0, int[] m1, int[] m2)
        {
            for (int k = 0; k < m0.Length; ++k)
                m2[k] = (m0[k] + m1[k]) % n[k];
        }

        public static int[][] AllPermutation(int order, bool addZero = true)
        {
            var pool = Enumerable.Range(1, order).ToList();
            var acc = new List<List<int>>() { new List<int>() };
            var tmpPool = new Queue<int>(pool);
            while (tmpPool.Count != 0)
            {
                var p = tmpPool.Dequeue();
                var tmpAcc = new List<List<int>>();
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

        public static int[] Canonic(int dim, int rank)
        {
            int[] table = new int[dim];
            table[rank] = 1;
            return table;
        }

        public static int[][] AllTuples(params int[] dims)
        {
            var acc = new List<List<int>>() { new List<int>() };
            for (int i = 0; i < dims.Length; ++i)
            {
                var tmpAcc = new List<List<int>>();
                foreach (var l0 in acc)
                {
                    for (int k = 0; k < dims[i]; ++k)
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
    }
}