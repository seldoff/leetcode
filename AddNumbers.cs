using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace Leetcode
{
    // https://leetcode.com/problems/add-two-numbers/
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }

        public override string ToString()
        {
            var r = new StringBuilder();
            var curr = this;
            while (curr != null)
            {
                r.Append(curr.val);
                curr = curr.next;
            }

            return r.ToString();
        }
    }

    public class AddNumbersSolution
    {
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            ListNode? x = l1;
            ListNode? y = l2;
            ListNode? curr = null;
            ListNode? tail = null;
            bool overflow = false;
            while (x != null || y != null)
            {
                var sum = (x?.val ?? 0) + (y?.val ?? 0);
                if (overflow)
                {
                    sum++;
                }

                var newNode = new ListNode(sum % 10);
                if (tail == null)
                {
                    tail = curr = newNode;
                }
                else
                {
                    curr!.next = newNode;
                    curr = newNode;
                }

                overflow = sum >= 10;
                x = x?.next;
                y = y?.next;
            }

            if (overflow)
            {
                curr.next = new ListNode(1);
            }

            return tail;
        }
    }

    [TestFixture]
    class AddNumbersSolutionTest
    {
        [Test]
        public void Test()
        {
            var s = new AddNumbersSolution();
            Console.WriteLine(s.AddTwoNumbers(
                new ListNode(2, new ListNode(4, new ListNode(3, null))),
                new ListNode(5, new ListNode(6, new ListNode(4, null)))));
        }
    }
}
