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
            var z20 = new Group256(20);
            z20.GenerateAll();
            var G = new GroupSubSet<byte, Integer256>(z20, z20.Elements(), name: "Z");
            var H = new Monogenic<byte, Integer256>(z20, z20.CreateElement(5), name: "5Z");
            G.DisplayHead();
            H.DisplayHead();

            var Qg = new GroupQuotient<byte, Integer256>(G, H, GroupOpLR.Left);
            Qg.Details();

            foreach (var e in Qg.ClassOf)
                e.Value.Display();
        }

        static void SamplesSigma()
        {
            var sn = new Sigma(4);
            sn.GenerateAll();
            var G = new GroupSubSet<byte, Permutation>(sn, sn.Elements(), "S4");
            G.DisplayHead();

            var H = new Monogenic<byte, Permutation>(sn, sn.kCycle(4), "H");
            H.Details();

            var Qg = new GroupQuotient<byte, Permutation>(G, H, GroupOpLR.Left);
            Qg.Details();

            foreach (var e in Qg.ClassOf)
                e.Value.Display();
        }

        public static void Main(string[] args)
        {
            //SamplesInteger256();
            SamplesSigma();
        }
    }
}
