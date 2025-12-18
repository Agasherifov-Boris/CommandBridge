using CommandBridge.Interfaces;
using CommandBridge.Models;
using System;
using System.Threading.Tasks;

namespace CommandBridge.Interceptors
{
    
    public class DelegatedInterceptor<TCommand, TResult> : ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly Func<ICommandContext<TCommand, TResult>, CommandDelegate<TCommand, TResult>, ValueTask<TResult>> _interceptorDelegate;

        #region Ctor

        public DelegatedInterceptor(Func<ICommandContext<TCommand, TResult>, CommandDelegate<TCommand, TResult>, ValueTask<TResult>> interceptorDelegate)
        {
            _interceptorDelegate = interceptorDelegate;
        }

        #endregion

        public ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next)
        {
            return _interceptorDelegate(context, next);
        }
    }
}