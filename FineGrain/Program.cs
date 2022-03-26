using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace FineGrain
{
    class MainClass
    {
        static void SamplesInteger256()
        {
            var zn = new Group256(20);
            var G = zn.Generate("Z", 1);
            var H = zn.Generate("5Z", 5);
            G.DisplayHead();
            H.DisplayHead();

            var Qg = new QuotientGroup<byte, Integer256>(G, H);
            Qg.Details();
            Qg.DisplayClasses();
        }

        static void SamplesSigma3()
        {
            var sn = new Sigma(3);
            var H0 = sn.Generate("H0", (1, 2, 3), (1, 2));
            var H1 = sn.Generate("H1", (1, 2, 3));
            H0.DisplayElements();
            H1.DisplayElements();

            var Qg0 = new QuotientGroup<byte, Permutation>(H0, H1);
            Qg0.Details();
            Qg0.DisplayClasses();
        }

        static void SamplesSigma4()
        {
            var sn = new Sigma(4);
            var H0 = sn.Generate("H0", (1, 3), (2, 4), Sigma.KCycle(4));
            var H1 = sn.Generate("H1", (1, 3), (2, 4));
            H0.DisplayElements();
            H1.DisplayElements();

            var Qg0 = new QuotientGroup<byte, Permutation>(H0, H1);
            Qg0.Details();
            Qg0.DisplayClasses();
        }

        static void SamplesSigma5()
        {
            var sn = new Sigma(5);
            var H0 = sn.Generate("H0", (1, 2, 3), (4, 5));
            var H1 = sn.Generate("H1", (4, 5));
            H0.DisplayElements();
            H1.DisplayElements();

            var Qg0 = new QuotientGroup<byte, Permutation>(H0, H1);
            Qg0.Details();
            Qg0.DisplayClasses();
        }

        static void SamplesAbelians()
        {
            var z3xz4 = new Abelian(3, 4);
            var G = z3xz4.Generate("G", (1, 0), (0, 1));
            var H = z3xz4.Generate("H", (0, 2));
            G.DisplayElements();
            H.DisplayElements();

            var QG = new QuotientGroup<byte, Tuple256>(G, H);
            QG.Details();
            QG.DisplayClasses();
        }

        static void Normalize()
        {
            var sn = new Sigma(4);
            var H0 = sn.Generate("H0", (1, 3), (2, 4), Sigma.KCycle(3));
            var S = sn.SubSet(sn.Table(3, 2, 1, 4));
            var N = new Normalize<byte, Permutation>(H0, S);
            H0.DisplayElements();
            S.DisplayElements();
            N.Details();
        }

        static void Centerize()
        {
            var sn = new Sigma(4);
            var H0 = sn.Generate("H0", (1, 3), (2, 4), Sigma.KCycle(3));
            var S = sn.SubSet(sn.Table(3, 2, 1, 4));
            var Z = new Centerize<byte, Permutation>(H0, S);
            H0.DisplayElements();
            S.DisplayElements();
            Z.Details();
        }

        public static void Main(string[] args)
        {
            //SamplesSigma5();
            //SamplesAbelians();
            Normalize();
            Centerize();
        }
    }
}
