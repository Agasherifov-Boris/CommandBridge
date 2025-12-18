using System.Threading;
using System.Threading.Tasks;

namespace CommandBridge.Interfaces
{
    public interface ICommandSender
    {
        ValueTask<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default);
    }
}