using CommandBridge.Interfaces;
using CommandBridge.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandBridge.Tests.Performance.Internal
{
    public class CommandDelegateFactory3 : ICommandDelegateFactory
    {
        public CommandDelegate<TResult> Create<TResult>(Type commandType)
        {
            var method = typeof(CommandDelegateFactory3)
                .GetMethod(nameof(BuildDelegate), new Type[0])!
                .MakeGenericMethod(commandType, typeof(TResult));

            var commandDelegate = (CommandDelegate<TResult>)method.Invoke(this, null)!;
            return commandDelegate;
        }

        public CommandDelegate<TResult> BuildDelegate<TCommand, TResult>()
            where TCommand : ICommand<TResult>
        {
            return (sp, cmd, ct) =>
            {
                var context = new CommandContext<TCommand, TResult>((TCommand)cmd, sp, ct);
                var interceptors = (ICommandInterceptor<TCommand, TResult>[])sp.GetServices<ICommandInterceptor<TCommand, TResult>>();

                return interceptors[4].HandleAsync(context, (ctx4) =>
                {
                    return interceptors[3].HandleAsync(context, (ctx3) => 
                    {
                        return interceptors[2].HandleAsync(context, (ctx2) =>
                        {
                            return interceptors[1].HandleAsync(context, (ctx1) =>
                            {
                                return interceptors[0].HandleAsync(context, (ctx0) =>
                                {
                                    var handler = ctx0.Services.GetRequiredService<ICommandHandler<TCommand, TResult>>();
                                    return handler.HandleAsync(ctx0.Command, ct);
                                });
                            });
                        });
                    });
                });
            };
        }

    }
}