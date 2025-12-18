using CommandBridge.Models;
using System.Threading.Tasks;

namespace CommandBridge.Interfaces
{
    public interface ICommandInterceptor<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        ValueTask<TResult> HandleAsync(ICommandContext<TCommand, TResult> context, CommandDelegate<TCommand, TResult> next);
    }
}
