using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var zn = new Group256(90);
            zn.CreateElement(1);

            var G = new GroupSubSet<byte, Integer256>(zn, zn.Elements(), "Z");
            G.DisplayHead();
            var m15 = zn.CreateElement(15);

            var H = new Monogenic<byte, Integer256>(zn, m15, "15Z");
            H.DisplayElements();

            var Gh = new GroupQuotient<byte, Integer256>(G, H, GroupOpLR.Left);
            Gh.Display();

            var m3 = Gh.Elements().ElementAt(3);
            var H1 = new Monogenic<byte, Integer256>(Gh, m3, "3Z", "|{0}| = {1} ");
            H1.Details();
            var Gh1 = new GroupQuotient<byte, Integer256>(Gh, H1, GroupOpLR.Left);
            Gh1.Details();

            var Dummy = new GroupQuotient<byte, Integer256>(Gh1);
            Dummy.Details();
        }
    }
}
