using CommandBridge.Interfaces;
using GenericCommands.Commands;

namespace GenericCommands.CommandHandlers;

public class SumCommandHandler<T> : ICommandHandler<SumCommand<T>, T>
{
    public ValueTask<T> HandleAsync(SumCommand<T> command, CancellationToken ct)
    {
        return ValueTask.FromResult((dynamic)command.A! + (dynamic)command.B!);
    }
}