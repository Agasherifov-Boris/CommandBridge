using System;
using System.Threading;

namespace CommandBridge.Interfaces
{
    public interface ICommandContext<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        IServiceProvider Services { get; }
        CancellationToken CancellationToken { get; }
        TCommand Command { get; }
    }
}