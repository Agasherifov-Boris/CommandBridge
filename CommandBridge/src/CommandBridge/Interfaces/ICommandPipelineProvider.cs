using CommandBridge.Models;

namespace CommandBridge.Interfaces
{
    public interface ICommandPipelineProvider
    {
        CommandDelegate<TResult> For<TResult>(ICommand<TResult> command);
    }
}