using CommandBridge.Attributes;
using CommandBridge.Interfaces;
using IntCommands.Commands;
using IntCommands.Interceptors;

namespace IntCommands.CommandHandlers
{
    [UseInterceptor(typeof(SumPositiveAlwaysInterceptor))]
    public class CalculateSumCommandHandler : ICommandHandler<CalculaceSumCommand, int>
    {
        public ValueTask<int> HandleAsync(CalculaceSumCommand command, CancellationToken ct)
        {
            var result = command.A + command.B;
            return ValueTask.FromResult(result);
        }
    }
}