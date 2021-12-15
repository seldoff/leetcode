using NUnit.Framework;

namespace Leetcode
{
    [TestFixture]
    public class SetPartitionTest
    {
        [Test]
        public void Test()
        {
            Assert.True(new SetPartition().CanPartition(new[] {1, 5, 11, 5}));
            Assert.False(new SetPartition().CanPartition(new[] {1, 2, 3, 5}));
            Assert.True(new SetPartition().CanPartition(new[] {1, 5, 10, 6}));
        }
    }
}
