using System;
using System.Collections.Generic;
using System.Linq;

namespace FineGrain
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!!!");
            var z256 = new Group256();
            var m32 = z256.CreateElement(32);
            var G = new GroupSubSet<byte, Integer256>(z256, z256.Elements(), "|G| = {0}");
            var H = new Monogenic<byte, Integer256>(z256, m32, "|H| = {0}");

            G.Details();
            H.Details();

            var Gh = new GroupQuotient<byte, Integer256>(G, H, GroupOpLR.Left);
            Gh.Details();

            var m4 = Gh.Elements().ElementAt(4);
            m4.Display();
            var H1 = new Monogenic<byte, Integer256>(Gh, m4, "|H| = {0}");
            H1.Details();
            var Gh1 = new GroupQuotient<byte, Integer256>(Gh.SubGroup, H1, GroupOpLR.Left);
            Gh1.Details();
        }
    }
}
