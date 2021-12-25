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
            foreach (var t in nums)
            {
                if (!freq.TryGetValue(t, out var count))
                {
                    count = 0;
                }

                count++;
                freq[t] = count;
            }

            var buckets = new List<int>?[nums.Length + 1];
            foreach (var key in freq.Keys)
            {
                var f = freq[key];
                buckets[f] ??= new List<int>();
                buckets[f]!.Add(key);
            }

            var r = new List<int>(k);
            for (int i = buckets.Length - 1; i >= 0 && r.Count < k; i--)
            {
                if (buckets[i] != null)
                {
                    r.AddRange(buckets[i]);
                }
            }

            return r.Take(k).ToArray();
        }
    }
}
