using CommandBridge.Tests.Models;

namespace CommandBridge.Tests.Internal.Tracking.Trackers
{
    public class InvokationOrderTracker(CounterCollection counters) : ITracker
    {
        private readonly List<int> _bucket = [];

        public IReadOnlyCollection<int> Bucket => _bucket.AsReadOnly();

        public void Track()
        {
            var counter = counters[nameof(InvokationOrderTracker)];
            var next = counter.GetNext();
            _bucket.Add(next);
        }

        public void Track(string key)
        {
            var counter = counters[$"{nameof(InvokationOrderTracker)}.{key}"];
            var next = counter.GetNext();
            _bucket.Add(next);
        }
    }
}