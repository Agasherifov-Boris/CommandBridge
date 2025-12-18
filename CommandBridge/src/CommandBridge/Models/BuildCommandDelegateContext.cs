using CommandBridge.Interfaces;
using System;

namespace CommandBridge.Models
{
    public class BuildCommandDelegateContext<TResult>
    {
        public BuildCommandDelegateContext(Type commandType) 
        { 
            ResultType = typeof(TResult);
            HandlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, ResultType);
            CommandType = typeof(ICommand<>).MakeGenericType(ResultType);
            CommandImplType = commandType;
            InputArgs = new CommandDelegateArguments<TResult>(commandType);
        }

        /// <summary>
        /// Interface type of command ICommand[TResult]
        /// </summary>
        public Type CommandType { get; }

        /// <summary>
        /// Command implementation type
        /// </summary>
        public Type CommandImplType { get; }

        /// <summary>
        /// Result type of command
        /// </summary>
        public Type ResultType { get; }

        /// <summary>
        /// Command handler type (interface), ICommandHandler[TCommand, TResult]
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// Delegate input args (IServiceProvider sp, ICommand[TResult] cmd, CancellationToken ct)
        /// </summary>
        public CommandDelegateArguments<TResult> InputArgs { get; }
    }
}