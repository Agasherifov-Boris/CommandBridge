using CommandBridge.Interfaces;
using CommandBridge.Models;

namespace CommandBridge.Tests.Performance.Models
{
    
    public class AsyncInterceptor1<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            var result = await next(context);

            return result;
        }
    }

    public class AsyncInterceptor2<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            var result = await next(context);

            return result;
        }
    }

    public class AsyncInterceptor3<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            var result = await next(context);

            return result;
        }
    }

    public class AsyncInterceptor4<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            var result = await next(context);

            return result;
        }
    }

    public class AsyncInterceptor5<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        public async ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            var result = await next(context);

            return result;
        }
    }

}