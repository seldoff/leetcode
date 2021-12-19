using System;
using System.Text;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/decode-string/
    public class DecodeStringSolution
    {
        public string DecodeString(string s)
        {
            var pos = 0;
            return Do(s, ref pos);
        }

        private string Do(string s, ref int pos)
        {
            var result = new StringBuilder(s.Length);
            while (pos < s.Length)
            {
                if (s[pos] == ']')
                {
                    break;
                }
                if (char.IsNumber(s[pos]))
                {
                    var count = ReadCount(s, ref pos);
                    var subString = ReadSubstring(s, ref pos);
                    for (int i = 0; i < count; i++)
                    {
                        result.Append(subString);
                    }
                }
                else
                {
                    result.Append(s[pos]);
                    pos++;
                }
            }

            return result.ToString();
        }

        private string ReadSubstring(string s, ref int pos)
        {
            pos++; // Skip [
            var result = Do(s, ref pos);
            pos++; // Skip ]
            return result;
        }

        private int ReadCount(string s, ref int pos)
        {
            var count = 0;
            while (char.IsNumber(s[pos]))
            {
                count = count * 10 + int.Parse(s[pos].ToString());
                pos++;
            }

            return count;
        }
    }

    [TestFixture]
    public class DecodeStringTest
    {
        [Test]
        public void Test()
        {
            var s = new DecodeStringSolution();
            Assert.AreEqual("aaabcbc", s.DecodeString("3[a]2[bc]"));
            Assert.AreEqual("accaccacc", s.DecodeString("3[a2[c]]"));
            Assert.AreEqual("abcabccdcdcdef", s.DecodeString("2[abc]3[cd]ef"));
            Assert.AreEqual("abccdcdcdxyz", s.DecodeString("abc3[cd]xyz"));
        }
    }
}
