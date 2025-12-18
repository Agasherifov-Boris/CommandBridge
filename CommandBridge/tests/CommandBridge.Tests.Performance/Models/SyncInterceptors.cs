using CommandBridge.Interfaces;
using CommandBridge.Models;

namespace CommandBridge.Tests.Performance.Models
{
    public class SyncInterceptor1<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
    public class SyncInterceptor2<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
    public class SyncInterceptor3<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
    public class SyncInterceptor4<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
    public class SyncInterceptor5<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return next(context);
        }
    }
}