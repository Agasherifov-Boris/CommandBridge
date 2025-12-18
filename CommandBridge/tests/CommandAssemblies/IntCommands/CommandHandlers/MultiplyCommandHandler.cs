using CommandBridge.Interfaces;
using IntCommands.Commands;

namespace IntCommands.CommandHandlers
{
    public class MultiplyCommandHandler : ICommandHandler<MultiplyCommand, int>
    {
        public ValueTask<int> HandleAsync(MultiplyCommand command, CancellationToken ct)
        {
            return ValueTask.FromResult(command.A * command.B);
        }
    }
}