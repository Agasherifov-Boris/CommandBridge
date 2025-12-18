using CommandBridge.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace CommandBridge.Models
{
    public class CommandDelegateArguments<TResult> : IEnumerable<ParameterExpression>
    {
        public CommandDelegateArguments(Type commandType) 
        {
            var resultType = typeof(TResult);
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);

            //ICommand<TResult>
            var cmdBaseType = typeof(ICommand<>).MakeGenericType(resultType);

            // Params: (IServiceProvider sp, TCommand cmd, CancellationToken ct)
            ServiceProvider = Expression.Parameter(typeof(IServiceProvider), "sp");
            Command = Expression.Parameter(cmdBaseType, "cmd");
            CancellationToken = Expression.Parameter(typeof(CancellationToken), "ct");
        }

        public ParameterExpression ServiceProvider { get; }
        public ParameterExpression Command  { get; }
        public ParameterExpression CancellationToken { get; }

        public IEnumerator<ParameterExpression> GetEnumerator()
        {
            yield return ServiceProvider;
            yield return Command;
            yield return CancellationToken;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}