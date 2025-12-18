using CommandBridge.Interfaces;
using CommandBridge.Tests.DemoProjects.Shop.Commands;
using CommandBridge.Tests.DemoProjects.Shop.Interfaces;

namespace CommandBridge.Tests.DemoProjects.Shop.Abstractions;

public abstract class CreateEntityCommandHandlerBase<TEntity>(IRepository<TEntity> repository) 
    : ICommandHandler<CreateEntityCommand<TEntity>, TEntity>
    where TEntity : Entity
{
    public async ValueTask<TEntity> HandleAsync(CreateEntityCommand<TEntity> command, CancellationToken ct)
    {
        var result = await repository.CreateAsync(command.Entity, ct);
        return result;
    }
}