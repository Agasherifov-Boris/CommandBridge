using CommandBridge.Tests.Internal.Tracking;
using CommandBridge.Tests.Internal.Tracking.Trackers;

namespace CommandBridge.Tests.Extensions
{
    public static class TrackingServiceExtensions
    {
        public static void TrackInvokationOrder<TOwner>(this TrackingService trackingService, string key)
        {
            var tracker = trackingService.Get<TOwner, InvokationOrderTracker>(key);
            tracker.Track(key);
        }

        public static void TrackInvokationOrder<TOwner>(this TrackingService<TOwner> trackingService, string key)
        {
            var tracker = trackingService.Get<InvokationOrderTracker>(key);
            tracker.Track(key);
        }
    }
}