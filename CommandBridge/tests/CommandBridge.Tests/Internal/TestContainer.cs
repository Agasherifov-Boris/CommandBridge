using CommandBridge.Interfaces;
using CommandBridge.Tests.Internal.Tracking;
using CommandBridge.Tests.Tools.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommandBridge.Tests.Internal
{
    public class TestContainer
    {
        public IServiceProvider Services { get; }
        public TrackingService TrackingService { get; }
        public ICommandSender CommandSender { get; }
        public Logger Logger { get; }

        #region Ctor

        public TestContainer(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
            CommandSender = serviceProvider.GetRequiredService<ICommandSender>();
            TrackingService = serviceProvider.GetRequiredService<TrackingService>();
            Logger = serviceProvider.GetRequiredService<Logger>();
        }

        #endregion

        public static TestContainer Create(Action<TestContainerBuilder> builder) 
        { 
            var containerBuilder = new TestContainerBuilder();
            builder(containerBuilder);

            var serviceProvider = containerBuilder.Build();

            return new TestContainer(serviceProvider);
        }
    }
}