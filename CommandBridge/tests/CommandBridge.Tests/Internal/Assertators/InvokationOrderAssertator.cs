using CommandBridge.Tests.Internal.Tracking.Trackers;

namespace CommandBridge.Tests.Internal.Assertators
{
    public class InvokationOrderAssertator(InvokationOrderTracker tracker)
    {
        public void ShouldBe(IEnumerable<int> expected)
        {
            Assert.Equal(expected, tracker.Bucket);
        }

        public void ShouldBeEmpty()
        {
            Assert.Empty(tracker.Bucket);
        }
    }
}