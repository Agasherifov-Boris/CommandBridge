using CommandBridge.Collections;
using CommandBridge.Interfaces;
using CommandBridge.Models;

namespace CommandBridge.Services
{
    public class CommandPipelineProvider : ICommandPipelineProvider
    {
        private readonly CommandPipelineCollection _pipelines;
        private readonly ICommandDelegateFactory _delegateFactory;

        #region Ctor

        public CommandPipelineProvider(CommandPipelineCollection pipelines, ICommandDelegateFactory delegateFactory)
        {
            _pipelines = pipelines;
            _delegateFactory = delegateFactory;
        }

        #endregion

        public virtual CommandDelegate<TResult> For<TResult>(ICommand<TResult> command)
        {
            return _pipelines.GetOrAdd<TResult>(command.GetType(), () => BuildPipeline<TResult>(command));
        }

        #region BuildPipeline

        protected virtual CommandDelegate<TResult> BuildPipeline<TResult>(ICommand<TResult> command)
        {
            return _delegateFactory.Create<TResult>(command.GetType());
        }

        #endregion
    }
}