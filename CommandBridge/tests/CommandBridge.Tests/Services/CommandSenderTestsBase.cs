using CommandBridge.Configuration;
using CommandBridge.Extensions;
using CommandBridge.Tests.Internal;

namespace CommandBridge.Tests.Services
{
    public abstract class CommandSenderTestsBase
    {
        #region CreateCommandSender

        protected TestContainer CreateContainer(Action<CommandBridgeConfigurationBuilder> opts)
        {
            return TestContainer.Create(builder => 
            {
                builder.Services.AddCommandBridge(opts);
            });
        }

        #endregion

    }
}