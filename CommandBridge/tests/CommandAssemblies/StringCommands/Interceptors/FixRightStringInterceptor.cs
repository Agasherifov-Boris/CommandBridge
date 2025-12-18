using CommandBridge.Interfaces;
using CommandBridge.Models;
using StringCommands.Commands;

namespace StringCommands.Interceptors
{
    public class FixRightStringInterceptor : ICommandInterceptor<ConcatonateCommand, string>
    {
        public static long LastCalledTicks { get; set; }

        public ValueTask<string> HandleAsync(ICommandContext<ConcatonateCommand, string> context, CommandDelegate<ConcatonateCommand, string> next)
        {
            LastCalledTicks = DateTime.UtcNow.Ticks;
            context.Command.Rigth = context.Command.Rigth.Trim();
            return next(context);
        }
    }
}