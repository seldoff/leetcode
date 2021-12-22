using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/minimum-remove-to-make-valid-parentheses
    public class MinimumRemoveParenthesis
    {
        public string MinRemoveToMakeValid(string s)
        {
            var stack = new Stack<int>();
            var oc = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '(':
                        stack.Push(i);
                        break;
                    case ')':
                        if (!stack.TryPop(out _))
                        {
                            oc.Add(i);
                        }
                        break;
                }
            }

            oc.AddRange(stack);
            var h = oc.ToHashSet();
            var r = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (!h.Contains(i))
                {
                    r.Append(s[i]);
                }
            }

            return r.ToString();
        }
    }

    [TestFixture]
    public class MinimumRemoveParenthesisTest
    {
        [Test]
        public void Test()
        {
            var s = new MinimumRemoveParenthesis();
            Assert.AreEqual("", s.MinRemoveToMakeValid(""));
            Assert.AreEqual("abc", s.MinRemoveToMakeValid("abc"));
            Assert.AreEqual("", s.MinRemoveToMakeValid(")))((("));
        }
    }
}
