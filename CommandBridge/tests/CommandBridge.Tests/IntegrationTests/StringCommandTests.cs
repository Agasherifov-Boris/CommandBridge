using CommandBridge.Tests.Internal;
using CommandBridge.Extensions;
using StringCommands.Commands;
using StringCommands.Interceptors;

namespace CommandBridge.Tests.IntegrationTests
{
    public class StringCommandTests
    {
        [Fact]
        public async Task ConcatonateCommand() 
        {
            var container = TestContainer.Create(builder =>
            {
                builder.Services.AddCommandBridge(cfg =>
                {
                    cfg.FromAssemblies(typeof(ConcatonateCommand).Assembly);
                });
            });

            var commandSender = container.CommandSender;
            var command = new ConcatonateCommand { Left = "  Hello,  ", Rigth = "  World!  " };
            var result = await commandSender.SendAsync(command);
            Assert.Equal("Hello, World!", result);

            // Check the order of interceptor calls
            Assert.True(FixLeftStringInterceptor.LastCalledTicks < FixRightStringInterceptor.LastCalledTicks);
        }
    }
}