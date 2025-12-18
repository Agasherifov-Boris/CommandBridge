using CommandBridge.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandBridge.Services
{
    public class CommandSender : ICommandSender
    {
        private readonly ICommandPipelineProvider _pipelineProvider;
        private readonly IServiceProvider _serviceProvider;

        #region Ctor

        public CommandSender(ICommandPipelineProvider pipelineProvider, IServiceProvider serviceProvider)
        {
            _pipelineProvider = pipelineProvider;
            _serviceProvider = serviceProvider;
        }

        #endregion

        public ValueTask<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default)
        {
            var pipeline = _pipelineProvider.For(command);
            return pipeline(_serviceProvider, command, ct);
        }
    }
}