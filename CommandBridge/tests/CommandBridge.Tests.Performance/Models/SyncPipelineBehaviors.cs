using CommandBridge.Interfaces;
using CommandBridge.Models;
using MediatR;

namespace CommandBridge.Tests.Performance.Models
{
    public class SyncPipelineBehavior1<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : notnull
    {
        public Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
    public class SyncPipelineBehavior2<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : notnull
    {
        public Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
    public class SyncPipelineBehavior3<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : notnull
    {
        public Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
    public class SyncPipelineBehavior4<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : notnull
    {
        public Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
    public class SyncPipelineBehavior5<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : notnull
    {
        public Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
}