using CommandBridge.Interfaces;
using System;
using System.Linq;
using System.Threading;
using CommandBridge.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CommandBridge.Extensions;
using Microsoft.Extensions.Options;
using CommandBridge.Configuration;

namespace CommandBridge.Services
{
    public class CommandDelegateFactory : ICommandDelegateFactory
    {
        private readonly IOptions<CommandBridgeConfiguration> _config;

        #region Ctor

        public CommandDelegateFactory(IOptions<CommandBridgeConfiguration> config)
        {
            _config = config;
        }

        #endregion

        public CommandDelegate<TResult> Create<TResult>(Type commandType)
        {
            var method = typeof(CommandDelegateFactory)
                .GetMethod(nameof(BuildDelegate), new Type[0])
                .MakeGenericMethod(commandType, typeof(TResult));

            var commandDelegate = (CommandDelegate<TResult>)method.Invoke(this, null);
            return commandDelegate;
        }

        public CommandDelegate<TResult> BuildDelegate<TCommand, TResult>()
            where TCommand : ICommand<TResult>
        {
            var commandConfiguration = _config.Value.Commands.Of(typeof(TCommand));
            var handlerType = commandConfiguration.HandlerType;

            if (handlerType.IsGenericTypeDefinition) 
            { 
                handlerType = handlerType.MakeGenericType(typeof(TResult));
            }

            var interceptors = _config.Value.Interceptors
                .Concat(commandConfiguration.Interceptors)
                .Select(CreateInterceptorFactory<TCommand, TResult>)
                .ToArray();

            //No interceptors, return direct command handler invokation
            if (interceptors.Length == 0)
                return (sp, cmd, ct) => InvokeCommand<TCommand, TResult>(sp, (TCommand)cmd, handlerType, ct);

            //only one interceptor, avoid array/enumerable allocation
            if (interceptors.Length == 1)
            {
                return (sp, cmd, ct) =>
                {
                    var interceptor = interceptors[0](sp);
                    return interceptor.HandleAsync(
                        new CommandContext<TCommand, TResult>((TCommand)cmd, sp, ct),
                        (ctx) => InvokeCommand<TCommand, TResult>(ctx.Services, ctx.Command, handlerType, ctx.CancellationToken));
                };
            }

            return (sp, cmd, ct) =>
            {
                var context = new CommandContext<TCommand, TResult>((TCommand)cmd, sp, ct);

                CommandDelegate<TCommand, TResult> current = (ctx) =>
                    InvokeCommand<TCommand, TResult>(ctx.Services, ctx.Command, handlerType, ctx.CancellationToken);

                for (var i = interceptors.Length - 1; i >= 0; i--)
                {
                    var interceptor = interceptors[i](sp);
                    var next = current;
                    current = (ctx) => interceptor.HandleAsync(ctx, next);
                }

                return current(context);
            };
        }

        private ValueTask<TResult> InvokeCommand<TCommand, TResult>(IServiceProvider serviceProvider, TCommand command, Type handlerType, CancellationToken ct)
            where TCommand : ICommand<TResult>
        { 
            var handler = (ICommandHandler<TCommand, TResult>)serviceProvider.GetRequiredService(handlerType);
            return handler.HandleAsync(command, ct);
        }

        private Func<IServiceProvider, ICommandInterceptor<TCommand, TResult>> CreateInterceptorFactory<TCommand, TResult>(object interceptorObj)
            where TCommand : ICommand<TResult>
        {
            if (interceptorObj is Type type)
            {
                if (type.IsGenericTypeDefinition)
                    type = type.MakeGenericType(typeof(TCommand), typeof(TResult));

                return (sp) => (ICommandInterceptor<TCommand, TResult>)sp.GetRequiredService(type);
            }
            else if (interceptorObj is ICommandInterceptor<TCommand, TResult> instance)
            {
                return (sp) => instance;
            }
            else 
            {
                throw new Exception($"Invalid interceptor type {interceptorObj.GetType().FullName}");
            }
        }

    }
}