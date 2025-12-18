using CommandBridge.Tests.Internal.Tracking;
using CommandBridge.Tests.Internal.Tracking.Trackers;
using CommandBridge.Tests.Models;
using CommandBridge.Tests.Tools.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommandBridge.Tests.Internal
{
    public class TestContainerBuilder
    {
        public IServiceCollection Services { get; }

        public TestContainerBuilder() 
        {
            Services = new ServiceCollection();
            Services.AddScoped<CounterCollection>();
            Services.AddScoped<TrackingService>();
            Services.AddScoped(typeof(TrackingService<>));
            Services.AddTransient<InvokationOrderTracker>();
            Services.AddSingleton<Logger>();
        }

        public IServiceProvider Build() 
        {
            return Services.BuildServiceProvider();
        }
    }
}