using CommandBridge.Tests.Internal;
using CommandBridge.Extensions;
using GenericCommands.Commands;
using CommandBridge.Configuration;
using GenericCommands.Interceptors;
using GenericCommands.CommandHandlers;
using CommandBridge.Tests.Tools.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommandBridge.Tests.IntegrationTests;

public class DynamicCommandTests
{
    [Fact]
    public async Task SumCommand_NoInterceptors() 
    {
        var scanOptions = new ScanAssemblyOptions() 
        { 
            Interceptors = false
        };
        var container = TestContainer.Create(builder => 
        { 
            builder.Services.AddCommandBridge(cfg =>
            {
                cfg.FromAssemblies(options: scanOptions, typeof(SumCommand<>).Assembly);
            });
        });

        var commandSender = container.CommandSender;

        var command = new SumCommand<int> { A = 3, B = 4 };
        var result = await commandSender.SendAsync(command);
        Assert.Equal(typeof(int), result.GetType());
        Assert.Equal(7, result);

        var commandDouble = new SumCommand<double> { A = 3.5, B = 4.5 };
        var resultDouble = await commandSender.SendAsync(commandDouble);
        Assert.Equal(typeof(double), resultDouble.GetType());
        Assert.Equal(8.0, resultDouble);

        Assert.Empty(container.Logger.Logs);
    }

    [Fact]
    public async Task SumCommand_GlobalInterceptor()
    {
        var container = TestContainer.Create(builder =>
        {
            builder.Services.AddCommandBridge(cfg =>
            {
                cfg.AddInterceptor(typeof(LoggingInterceptor<,>));
                cfg.FromAssemblies(typeof(SumCommand<>).Assembly);
            });
        });

        var commandSender = container.CommandSender;

        var command = new SumCommand<int> { A = 3, B = 4 };
        var result = await commandSender.SendAsync(command);
        Assert.Equal(typeof(int), result.GetType());
        Assert.Equal(7, result);

        var commandDouble = new SumCommand<double> { A = 3.5, B = 4.5 };
        var resultDouble = await commandSender.SendAsync(commandDouble);
        Assert.Equal(typeof(double), resultDouble.GetType());
        Assert.Equal(8.0, resultDouble);

        Assert.Equal(2, container.Logger.Logs.Count);

        string[] expectedLogs = 
        [
            "Command SumCommand`1 executed with result: 7",
            "Command SumCommand`1 executed with result: 8"
        ];

        Assert.Equivalent(expectedLogs, container.Logger.Logs);
    }

    [Fact]
    public async Task SumCommand_FuncInterceptor()
    {
        var container = TestContainer.Create(builder =>
        {
            builder.Services.AddCommandBridge(cfg =>
            {
                cfg.AddCommand<SumCommand<int>, SumCommandHandler<int>, int>()
                    .WithInterceptor(async (ctx, next) => 
                    {
                        var result = await next(ctx);
                        var logger = ctx.Services.GetRequiredService<Logger>();
                        logger.Log($"Command SumCommand<int> executed with result: {result}");
                        return result;
                    });
            });
        });

        var commandSender = container.CommandSender;

        var command = new SumCommand<int> { A = 3, B = 4 };
        var result = await commandSender.SendAsync(command);
        Assert.Equal(typeof(int), result.GetType());
        Assert.Equal(7, result);

        command = new SumCommand<int> { A = 6, B = 8 };
        result = await commandSender.SendAsync(command);
        Assert.Equal(typeof(int), result.GetType());
        Assert.Equal(14, result);

        Assert.Equal(2, container.Logger.Logs.Count);

        string[] expectedLogs =
        [
            "Command SumCommand<int> executed with result: 7",
            "Command SumCommand<int> executed with result: 14"
        ];

        Assert.Equivalent(expectedLogs, container.Logger.Logs);
    }
}