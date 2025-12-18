using CommandBridge.Tests.DemoProjects.Shop.Abstractions;

namespace CommandBridge.Tests.DemoProjects.Shop.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
    ValueTask<TEntity> CreateAsync(TEntity entity, CancellationToken ct);
    ValueTask<TEntity> UpdateAsync(TEntity entity, CancellationToken ct);
    ValueTask DeleteAsync(Guid id, CancellationToken ct);
}
