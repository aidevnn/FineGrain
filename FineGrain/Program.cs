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

        static void SamplesSigma6()
        {
            var sn = new Sigma(6);
            var H0 = sn.Generate("H0", (1, 2, 3), (4, 5));
            var H1 = sn.Generate("H1", (4, 5));
            H0.DisplayElements();
            H1.DisplayElements();

            var Qg0 = new QuotientGroup<byte, Permutation>(H0, H1);
            Qg0.Details();
            Qg0.DisplayClasses();
        }

        public static void Main(string[] args)
        {
            SamplesInteger256();
            //SamplesSigma6();
        }
    }
}
