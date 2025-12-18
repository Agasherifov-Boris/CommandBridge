using CommandBridge.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBridge.Models
{
    public delegate ValueTask<TResult> CommandDelegate<TResult>(IServiceProvider serviceProvider, ICommand<TResult> command, CancellationToken ct);

    public delegate ValueTask<TResult> CommandDelegate<TCommand, TResult>(ICommandContext<TCommand, TResult> context)
        where TCommand : ICommand<TResult>;
}