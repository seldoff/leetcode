#define DBG

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/lfu-cache
    public class LFUCache
    {
        class UsageNode
        {
            public UsageNode? Prev { get; set; }
            public UsageNode? Next { get; set; }
            public int Counter { get; set; }
            public List<Node> Nodes { get; } = new List<Node>();
        }

        class Node
        {
            public int Key { get; set; }
            public int Value { get; set; }
            public int Counter { get; set; }

            public override string ToString() => $"{Key}:{Value}";
        }

        private readonly int _capacity;
        private readonly Dictionary<int, Node> _map;
        private readonly UsageNode _tail = new UsageNode {Counter = 1};

        public LFUCache(int capacity)
        {
            Console.WriteLine("New");
            _map = new Dictionary<int, Node>(capacity);
            _capacity = capacity;
        }

        public int Get(int key)
        {
            if (_map.TryGetValue(key, out var node))
            {
                IncCounter(node);
                #if DBG
                Console.WriteLine($"Get {key} - {DebugPrint()}");
                #endif
                return node.Value;
            }

            return -1;
        }

        public void Put(int key, int value)
        {
            if (_capacity == 0)
            {
                return;
            }

            if (_map.TryGetValue(key, out var node))
            {
                node.Value = value;
                IncCounter(node);
            }
            else
            {
                if (_map.Count == _capacity)
                {
                    Evict();
                }

                var newNode = new Node
                {
                    Key = key, Value = value, Counter = 1,
                };
                _tail.Nodes.Add(newNode);
                _map[key] = newNode;
            }

            #if DBG
            Console.WriteLine($"Put {key}:{value} - {DebugPrint()}");
            #endif
        }

        private string DebugPrint()
        {
            return string.Join(", ",
                _map.Values.OrderByDescending(n => n.Counter).Select(n => $"{n.Key}:{n.Counter}"));
        }

        private void IncCounter(Node node)
        {
            var usageNode = _tail;
            while (usageNode != null)
            {
                if (usageNode.Counter == node.Counter)
                {
                    usageNode.Nodes.Remove(node);
                    break;
                }
                usageNode = usageNode.Next;
            }

            node.Counter++;

            while (usageNode != null)
            {
                if (usageNode.Counter == node.Counter)
                {
                    usageNode.Nodes.Add(node);
                    break;
                }

                if (usageNode.Counter > node.Counter)
                {
                    var newUsageNode = new UsageNode
                    {
                        Counter = node.Counter, Prev = usageNode.Prev, Next = usageNode, Nodes = {node}
                    };
                    usageNode.Prev = newUsageNode;
                    newUsageNode.Prev!.Next = newUsageNode;
                    break;
                }

                if (usageNode.Next == null)
                {
                    var newUsageNode = new UsageNode
                    {
                        Counter = node.Counter, Prev = usageNode, Nodes = {node}
                    };
                    usageNode.Next = newUsageNode;
                    break;
                }

                usageNode = usageNode.Next;
            }
        }

        private void Evict()
        {
            var usageNode = _tail;
            while (usageNode != null)
            {
                if (usageNode.Nodes.Count > 0)
                {
                    var nodeToEvict = usageNode.Nodes[0];
                    _map.Remove(nodeToEvict.Key);
                    usageNode.Nodes.Remove(nodeToEvict);
                    break;
                }
                usageNode = usageNode.Next;
            }
        }
    }

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
