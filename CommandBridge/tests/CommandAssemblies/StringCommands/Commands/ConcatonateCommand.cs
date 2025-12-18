
using CommandBridge.Interfaces;

namespace StringCommands.Commands
{
    public class ConcatonateCommand : ICommand<string>
    {
        public required string Left { get; set; }
        public required string Rigth { get; set; }
    }
}