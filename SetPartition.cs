using System;
using System.Collections.Generic;
using System.Linq;

namespace Leetcode
{
    // https://leetcode.com/problems/partition-equal-subset-sum/
    public class SetPartition
    {
        public bool CanPartition(int[] nums)
        {
            var sum = nums.Sum();
            if (sum % 2 == 1)
            {
                return false;
            }

            sum /= 2;
            var flags = new bool[nums.Length + 1, sum + 1]; // [i, j] is true if first i elements can add up to j or more
            flags[0, 0] = true;
            return Do(nums, sum, flags);
        }

        private bool Do(int[] nums, int sum, bool[,] flags)
        {
            for (int i = 1; i <= nums.Length; i++)
            {
                var n = nums[i - 1];
                for (int j = 1; j <= sum; j++)
                {
                    if (n <= j)
                    {
                        // true if (i - 1) first elements already can add up to j OR
                        //      if (i - 1) first elements can add up to (j - ith element). So with ith element, first
                        //      first i elements add up to j.
                        flags[i, j] = flags[i - 1, j] || flags[i - 1, j - n];
                    }
                    else
                    {
                        flags[i, j] = flags[i - 1, j];
                    }
                }
            }

            return flags[nums.Length, sum];
        }
    }
}
