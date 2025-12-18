using MediatR;

namespace CommandBridge.Tests.Performance.Models
{

    public class AsyncPipelineBehavior1<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class AsyncPipelineBehavior2<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class AsyncPipelineBehavior3<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class AsyncPipelineBehavior4<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }

    public class AsyncPipelineBehavior5<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : notnull
    {
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            return result;
        }
    }
}