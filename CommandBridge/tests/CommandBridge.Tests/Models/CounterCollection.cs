using System.Collections.Concurrent;

namespace CommandBridge.Tests.Models
{
    public class CounterCollection
    {
        private ConcurrentDictionary<string, Counter> _counters = new();

        public Counter this[string name] => _counters.GetOrAdd(name, _ => new Counter());
    }
}