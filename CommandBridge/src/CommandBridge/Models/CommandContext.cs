using CommandBridge.Interfaces;
using System;
using System.Threading;

namespace CommandBridge.Models
{
    public class CommandContext<TCommand, TResult> : ICommandContext<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        #region Ctor

        public CommandContext(TCommand command, IServiceProvider serviceProvider, CancellationToken ct)
        {
            Command = command;
            CancellationToken = ct;
            Services = serviceProvider;
        }

        #endregion

        public TCommand Command { get; }
        public IServiceProvider Services { get; }
        public CancellationToken CancellationToken { get; }
    }
}