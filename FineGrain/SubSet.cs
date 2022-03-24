using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    public abstract class SubFSet<T, U> where U : Elt<T>, IComparable<Elt<T>> where T : struct, IComparable<T>, IEquatable<T>
    {
        protected static List<string> GenLetters(int n, bool skipFirst = false)
        {
            int skip = skipFirst ? 1 : 0;
            if (n > 50)
                return Enumerable.Range(skip + 1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Skip(skip).Take(n).Select(c => $"{c}").ToList();
        }

        protected SortedSet<U> elements;
        public FSet<T> FSet { get; }
        public List<U> Elements => elements.ToList();
        public bool Contains(U e) => elements.Contains(e);
        public int Count => elements.Count;

        protected SubFSet(FSet<T> fSet)
        {
            FSet = fSet;
            elements = new SortedSet<U>();
        }

        protected SubFSet(U elt)
        {
            FSet = elt.FSet;
            elements = new SortedSet<U>() { elt };
        }

        protected void Add(U e) => elements.Add(e);

        protected void AddRange(IEnumerable<U> collection, bool clearFirst = false)
        {
            if (clearFirst)
                elements.Clear();

            foreach (var e in collection)
            {
                if (e.FSet.HashCode == FSet.HashCode)
                    elements.Add(e);
            }
        }

        public virtual void DisplayHead()
        {
            Console.WriteLine(FSet.Fmt, Elements.Count);
        }

        public virtual void DisplayElements(bool skipFirst = false)
        {
            if (elements.Count == 0)
            {
                Console.WriteLine("Empty Set");
                return;
            }

            DisplayHead();

            if (Elements.Count > 200)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(Elements.Count, skipFirst);
            for (int k = 0; k < Elements.Count; ++k)
                Elements.ElementAt(k).Display(word[k]);

            Console.WriteLine();
        }

    }
}
