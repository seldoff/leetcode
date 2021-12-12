using NUnit.Framework;

namespace Leetcode
{
    [TestFixture]
    public static class LRUCacheTest
    {
        [Test]
        public static void Test()
        {
            var c = new LRUCache(2);
            Assert.AreEqual(-1, c.Get(1));

            c.Put(1, 10);
            Assert.AreEqual(10, c.Get(1));

            c.Put(2, 20);
            Assert.AreEqual(10, c.Get(1));
            Assert.AreEqual(20, c.Get(2));

            c.Put(3, 30);
            Assert.AreEqual(-1, c.Get(1));
            Assert.AreEqual(20, c.Get(2));
            Assert.AreEqual(30, c.Get(3));

            c.Get(2);
            c.Put(4, 40);
            Assert.AreEqual(-1, c.Get(3));
            Assert.AreEqual(20, c.Get(2));
            Assert.AreEqual(40, c.Get(4));
        }
    }
}
