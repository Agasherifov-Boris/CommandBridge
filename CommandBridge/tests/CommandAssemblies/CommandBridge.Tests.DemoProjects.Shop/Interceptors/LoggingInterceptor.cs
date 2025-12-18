using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Tests.Tools.Services;

namespace CommandBridge.Tests.DemoProjects.Shop.Interceptors;

public class LoggingInterceptor<TCommand, TResult>(Logger logger) : ICommandInterceptor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
    {
        logger.Log($"Starting execution of command {typeof(TCommand).Name}");
        try
        {
            var result = await next(context);
            logger.Log($"Successfully executed command {typeof(TCommand).Name}");
            return result;
        }
        catch (Exception ex)
        {
            logger.Log($"Error executing command {typeof(TCommand).Name}: {ex.Message}");
            throw;
        }
    }
}