using CommandBridge.Tests.Internal.Assertators;
using CommandBridge.Tests.Internal.Tracking;
using CommandBridge.Tests.Internal.Tracking.Trackers;

namespace CommandBridge.Tests.Extensions
{
    public static class TrackingServiceAssertExtensions
    {
        public static InvokationOrderAssertator InvokationOrderOf<TCategory>(this TrackingService trackingService, string key)
        {
            var tracker = trackingService.Get<TCategory, InvokationOrderTracker>(key);
            return new InvokationOrderAssertator(tracker);
        }

    }
}