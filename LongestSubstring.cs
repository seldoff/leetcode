using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/longest-substring-without-repeating-characters/
    public class LongestSubstring
    {
        public int LengthOfLongestSubstring(string s)
        {
            var sub = new Dictionary<char, int>();
            var currLength = 0;
            var maxLength = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (sub.TryGetValue(s[i], out var idx))
                {
                    if (maxLength < currLength)
                    {
                        maxLength = currLength;
                    }

                    foreach (var pair in sub.ToList().Where(pair => pair.Value < idx))
                    {
                        sub.Remove(pair.Key);
                    }
                    sub[s[i]] = i;
                    currLength = i - idx;
                }
                else
                {
                    sub.Add(s[i], i);
                    currLength++;
                }
            }

            return Math.Max(maxLength, currLength);
        }

        private class Node
        {
            public char Char { get; init; }
            public Node? Next { get; set; }

            public override string ToString() => Char.ToString();
        }

        public int LengthOfLongestSubstring2(string s)
        {
            Node? tail = null;
            Node? head = null;
            var currLength = 0;
            var maxLength = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (FindAndCutTail(s[i], ref tail, out var idx))
                {
                    if (maxLength < currLength)
                    {
                        maxLength = currLength;
                    }

                    if (tail == null)
                    {
                        head = null;
                    }

                    currLength -= idx;
                }
                else
                {
                    currLength++;
                }

                if (tail == null)
                {
                    tail = head = new Node {Char = s[i]};
                }
                else
                {
                    var prev = head;
                    head = new Node {Char = s[i]};
                    prev!.Next = head;
                }
            }

            return Math.Max(maxLength, currLength);
        }

        private bool FindAndCutTail(char c, ref Node? tail, out int idx)
        {
            idx = -1;
            var curr = tail;
            while (curr != null)
            {
                idx++;
                if (curr.Char == c)
                {
                    tail = curr.Next;
                    return true;
                }
                curr = curr.Next;
            }

            return false;
        }

        public int LengthOfLongestSubstring3(string s)
        {
            var max = 0;
            var d = new Dictionary<char, int>();
            var lastBreak = -1;
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (d.TryGetValue(c, out var idx))
                {
                    lastBreak = Math.Max(lastBreak, idx);
                }

                max = Math.Max(max, i - lastBreak);
                d[c] = i;
            }

            return max;
        }
    }

    [TestFixture]
    [ShortRunJob]
    [MemoryDiagnoser]
    public class LongestSubstringTest
    {
        [Test]
        public void Test()
        {
            var s = new LongestSubstring();
            Assert.AreEqual(3, s.LengthOfLongestSubstring3("abcabcbb"));
            Assert.AreEqual(1, s.LengthOfLongestSubstring3("bbbbb"));
            Assert.AreEqual(3, s.LengthOfLongestSubstring3("pwwkew"));
            Assert.AreEqual(2, s.LengthOfLongestSubstring3("abba"));
        }

        private readonly LongestSubstring s = new LongestSubstring();
        private string input = "A special argument named \"x-match\", added in the binding between exchange and queue, specifies if all headers must match or just one. Either any common header between the message and the binding count as a match, or all the headers referenced in the binding need to be present in the message for it to match. The \"x-match\" property can have two different values: \"any\" or \"all\", where \"all\" is the default value. A value of \"all\" means all header pairs (key, value) must match, while value of \"any\" means at least one of the header pairs must match. Headers can be constructed using a wider range of data types, integer or hash for example, instead of a string. The headers exchange type (used with the binding argument \"any\") is useful for directing messages which contain a subset of known (unordered) criteria. ";

        [Test]
        public void Bench()
        {
            BenchmarkRunner.Run<LongestSubstringTest>(DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
        }

        [Benchmark(Baseline = true)]
        public int V1() => s.LengthOfLongestSubstring(input);

        [Benchmark]
        public int V2() => s.LengthOfLongestSubstring2(input);

        [Benchmark]
        public int V3() => s.LengthOfLongestSubstring3(input);
    }
}
