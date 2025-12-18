using CommandBridge.Tests.Internal.Tracking.Trackers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace CommandBridge.Tests.Internal.Tracking
{
    public class TrackingService(IServiceProvider serviceProvider)
    {
        private ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _meters = new();

        public TTracker Get<TCategory, TTracker>(string name)
            where TTracker : ITracker
        {
            if (!_meters.TryGetValue(typeof(TCategory), out var entry))
            { 
                entry = new ConcurrentDictionary<string, object>();
                _meters[typeof(TCategory)] = entry;
            }

            if (!entry.TryGetValue(name, out var tracker))
            {
                tracker = serviceProvider.GetRequiredService<TTracker>();
                entry[name] = tracker;
            }

            return (TTracker)tracker;
        }
    }

    public class TrackingService<TCategory>(TrackingService collector)
    {
        public TTracker Get<TTracker>(string name)
            where TTracker : ITracker
        {
            return collector.Get<TCategory, TTracker>(name);
        }
    }
}