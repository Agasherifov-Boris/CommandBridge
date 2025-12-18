using CommandBridge.Attributes;
using CommandBridge.Interfaces;
using CommandBridge.Models;
using IntCommands.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace IntCommands.Interceptors
{
    [Lifetime(ServiceLifetime.Singleton)]
    public class SumPositiveAlwaysInterceptor : ICommandInterceptor<CalculaceSumCommand, int>
    {
        public static int InstancesCreated { get; set; } = 0;

        public SumPositiveAlwaysInterceptor()
        {
            InstancesCreated++;
        }

        public ValueTask<int> HandleAsync(ICommandContext<CalculaceSumCommand, int> context, CommandDelegate<CalculaceSumCommand, int> next)
        {
            if (context.Command.A < 0 || context.Command.B < 0)
                return ValueTask.FromResult(0);

            return next(context);
        }
    }
}