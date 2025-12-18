using CommandBridge.Interfaces;
using CommandBridge.Models;
using CommandBridge.Tests.Tools.Services;

namespace GenericCommands.Interceptors;

public class LoggingInterceptor<TCommand, TResult>(Logger logger) : ICommandInterceptor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{

    public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
    {
        var result = await next(context);
        logger.Log($"Command {typeof(TCommand).Name} executed with result: {result}");
        return result;
    }
}