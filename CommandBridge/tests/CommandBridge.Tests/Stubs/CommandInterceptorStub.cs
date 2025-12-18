using CommandBridge.Interfaces;
using CommandBridge.Models;

namespace CommandBridge.Tests.Stubs
{
    public class CommandInterceptorStub<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
            where TCommand : ICommand<TResult>
    {
        public virtual ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
}