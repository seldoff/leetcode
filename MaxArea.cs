using System;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/container-with-most-water
    public class MaxAreaSolution
    {
        public int MaxArea(int[] height)
        {
            var maxArea = A(0, 1);
            var maxAreaLeft = 0;
            var maxAreaWidth = 1;
            for (int i = 2; i < height.Length; i++)
            {
                var maxAreaIteration = 0;
                var maiLeft = -1;
                var width = i - maxAreaLeft;
                var widthDiff = width - maxAreaWidth;
                var upperBound = i - widthDiff;
                for (int j = 0; j <= upperBound; j++)
                {
                    var area = A(j, i);
                    if (area > maxAreaIteration)
                    {
                        maxAreaIteration = area;
                        maiLeft = j;
                    }
                }

                if (maxAreaIteration > maxArea)
                {
                    maxArea = maxAreaIteration;
                    maxAreaLeft = maiLeft;
                    maxAreaWidth = i - maxAreaLeft;
                }
            }

            return maxArea;

            int A(int l, int r) => (r - l) * Math.Min(height[r], height[l]);
        }

        public int MaxArea2(int[] height)
        {
            var max = 0;
            var maxIdx = 0;
            for (int i = 0; i < height.Length; i++)
            {
                var x = (i + 1) * height[i];
                if (x > max)
                {
                    max = x;
                    maxIdx = i;
                }
            }

            var max2 = 0;
            var maxIdx2 = 0;
            for (int i = 0; i < height.Length; i++)
            {
                var x = (height.Length - i) * height[i];
                if (x > max2 && maxIdx2 != maxIdx)
                {
                    max2 = x;
                    maxIdx2 = i;
                }
            }

            return Math.Abs(maxIdx2 - maxIdx) * Math.Min(height[maxIdx], height[maxIdx2]);
        }

        public int MaxArea3(int[] height)
        {
            var max = 0;
            var l = 0;
            var r = height.Length - 1;
            do
            {
                var m = A(l, r);
                if (m > max)
                {
                    max = m;
                }

                if (height[l] < height[r])
                {
                    l++;
                }
                else
                {
                    r--;
                }
            } while (l <= r);

            return max;
            int A(int l, int r) => (r - l) * Math.Min(height[r], height[l]);
        }
    }

    [TestFixture]
    public class MaxAreaSolutionTest
    {
        [Test]
        public void Test()
        {
            var s = new MaxAreaSolution();
            Assert.AreEqual(49, s.MaxArea3(new[] {1, 8, 6, 2, 5, 4, 8, 3, 7}));
            Assert.AreEqual(1, s.MaxArea3(new[] {1, 1}));
            Assert.AreEqual(17, s.MaxArea3(new[] {2, 3, 4, 5, 18, 17, 6}));
            Assert.AreEqual(2, s.MaxArea3(new[] {1, 2, 1}));
        }
    }
}
