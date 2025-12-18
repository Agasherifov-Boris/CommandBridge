using CommandBridge.Interfaces;
using MediatR;

namespace CommandBridge.Tests.Performance.Models
{
    public class CalculateSumCommand(int a, int b) : ICommand<int>, IRequest<int>
    {
        public int A { get; } = a;
        public int B { get; } = b;
    }

    public class CalculateSumRequest(int a, int b) : IRequest<int> 
    {
        public int A { get; } = a;
        public int B { get; } = b;
    }

    public class CalculateSumCommandHandler : ICommandHandler<CalculateSumCommand, int>
    {
        public ValueTask<int> HandleAsync(CalculateSumCommand command, CancellationToken ct)
        {
            return ValueTask.FromResult(command.A + command.B);
        }
    }

    public class CalculateSumRequestHandler : IRequestHandler<CalculateSumRequest, int>
    {
        public Task<int> Handle(CalculateSumRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.A + request.B);
        }
    }
}