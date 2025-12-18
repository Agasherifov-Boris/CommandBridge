using CommandBridge.Tests.Internal;
using CommandBridge.Extensions;
using IntCommands.Commands;
using IntCommands.Interceptors;

namespace CommandBridge.Tests.IntegrationTests;

public class IntCommandTests
{
    [Fact]
    public async Task CalculaceSumCommand() 
    {
        var container = TestContainer.Create(builder => 
        { 
            builder.Services.AddCommandBridge(cfg =>
            {
                cfg.FromAssemblies(typeof(CalculaceSumCommand).Assembly);
            });
        });

        var commandSender = container.CommandSender;

        SumPositiveAlwaysInterceptor.InstancesCreated = 0;

        var command = new CalculaceSumCommand { A = 3, B = 4 };
        var result = await commandSender.SendAsync(command);
        Assert.Equal(7, result);

        command = new CalculaceSumCommand { A = -3, B = -4 };
        result = await commandSender.SendAsync(command);
        Assert.Equal(0, result); // Because SumPositiveAlwaysInterceptor

        var interceptorInstances = SumPositiveAlwaysInterceptor.InstancesCreated;
        Assert.Equal(1, interceptorInstances); // Singleton
    }

    [Fact]
    public async Task MultiplyCommand()
    {
        var container = TestContainer.Create(builder =>
        {
            builder.Services.AddCommandBridge(cfg =>
            {
                cfg.FromAssemblies(typeof(CalculaceSumCommand).Assembly);
            });
        });

        var commandSender = container.CommandSender;

        var command = new MultiplyCommand { A = 3, B = 4 };
        var result = await commandSender.SendAsync(command);
        Assert.Equal(12, result);
    }
}