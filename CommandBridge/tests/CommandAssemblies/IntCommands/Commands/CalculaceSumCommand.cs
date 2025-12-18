using CommandBridge.Interfaces;

namespace IntCommands.Commands
{
    public class CalculaceSumCommand : ICommand<int>
    {
        public required int A { get; init; }
        public required int B { get; init; }
    }
}