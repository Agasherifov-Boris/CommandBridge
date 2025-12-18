using System.Threading;
using System.Threading.Tasks;

namespace CommandBridge.Interfaces
{

    public interface ICommandHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        ValueTask<TResult> HandleAsync(TCommand command, CancellationToken ct);
    }
}
