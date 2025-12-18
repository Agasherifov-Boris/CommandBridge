using CommandBridge.Attributes;
using CommandBridge.Interfaces;
using StringCommands.Commands;
using StringCommands.Interceptors;

namespace StringCommands.CommandHandlers
{
    [UseInterceptor(typeof(FixLeftStringInterceptor))]
    [UseInterceptor(typeof(FixRightStringInterceptor))]
    public class ConcatonateCommandHandler : ICommandHandler<ConcatonateCommand, string>
    {
        public ValueTask<string> HandleAsync(ConcatonateCommand command, CancellationToken ct)
        {
            return ValueTask.FromResult($"{command.Left} {command.Rigth}");
        }
    }
}