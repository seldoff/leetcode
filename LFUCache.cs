#define DBG

using System;
using System.Collections.Generic;
using System.Linq;

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
}
