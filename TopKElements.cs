using System.Collections.Generic;
using System.Linq;

namespace Leetcode
{
    // https://leetcode.com/problems/top-k-frequent-elements/
    public class TopKElements
    {
        public int[] TopKFrequent(int[] nums, int k)
        {
            var freq = new Dictionary<int, int>();
            var top = new Dictionary<int, int>();
            foreach (var t in nums)
            {
                if (!freq.TryGetValue(t, out var count))
                {
                    count = 0;
                }

                count++;
                freq[t] = count;

                if (top.ContainsKey(t) || top.Count < k)
                {
                    top[t] = count;
                }
                else
                {
                    foreach (var p in top.Keys)
                    {
                        if (top[p] < count)
                        {
                            top.Remove(p);
                            top[t] = count;
                            break;
                        }
                    }
                }
            }

            return top.Keys.ToArray();
        }
    }
}
