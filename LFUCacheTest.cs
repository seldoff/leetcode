using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Leetcode
{
    [TestFixture]
    public class LFUCacheTest
    {
        [Test]
        public void Test()
        {
            var c = new LFUCache(2);
            Assert.AreEqual(-1, c.Get(1));

            c.Put(1, 10);
            Assert.AreEqual(10, c.Get(1));

            c.Put(2, 20);
            Assert.AreEqual(20, c.Get(2));

            c = new LFUCache(2);
            c.Put(1, 10);
            c.Put(2, 20);
            c.Get(2);
            c.Put(3, 30);

            Assert.AreEqual(-1, c.Get(1));
            Assert.AreEqual(20, c.Get(2));
            Assert.AreEqual(30, c.Get(3));

            c = new LFUCache(2);
            c.Put(1, 10);
            c.Put(2, 20);
            c.Get(1);
            c.Get(2);
            c.Put(3, 30);

            Assert.AreEqual(-1, c.Get(1));
            Assert.AreEqual(20, c.Get(2));
            Assert.AreEqual(30, c.Get(3));
        }

        [Test]
        public void Test2()
        {
            var c = new LFUCache(2);
            c.Put(1, 1);
            c.Put(2, 2);
            c.Get(1);
            c.Put(3, 3);
            c.Get(2);
            c.Get(3);
            c.Put(4, 4);

            Assert.AreEqual(-1, c.Get(1));
            Assert.AreEqual(3, c.Get(3));
            Assert.AreEqual(4, c.Get(4));
        }

        [Test]
        public void Test3()
        {
            var lines = File.ReadAllLines("c:\\Users\\akruglik\\Documents\\projects\\leetcode\\testcase");
            var commands = lines[0].Split(",");
            var args = lines[1].Split(",").Select(s =>
            {
                s = s.Replace("[", "").Replace("]", "");
                return s.Split(",").Select(int.Parse).ToArray();
            }).ToArray();

            for (int i = 0; i < 1000; i++)
            {
                Inner(commands, args);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Inner(string[] commands, int[][] args)
        {
            var c = new LFUCache(10000);

            for (int i = 0; i < commands.Length; i++)
            {
                switch (commands[i])
                {
                    case "get":
                    {
                        c.Get(args[i][0]);
                        break;
                    }
                    case "put":
                    {
                        c.Put(args[i][0], args[i][1]);
                        break;
                    }
                }
            }
        }
    }
}
