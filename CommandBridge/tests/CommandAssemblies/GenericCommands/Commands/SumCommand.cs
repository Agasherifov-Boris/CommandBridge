

using CommandBridge.Interfaces;

namespace GenericCommands.Commands;

public class SumCommand<T> : ICommand<T>
{
    public required T A { get; init; }
    public required T B { get; init; }
}