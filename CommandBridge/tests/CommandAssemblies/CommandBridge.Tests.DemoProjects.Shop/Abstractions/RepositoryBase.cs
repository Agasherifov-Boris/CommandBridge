using CommandBridge.Tests.DemoProjects.Shop.Interfaces;
using System.Collections.Concurrent;

namespace CommandBridge.Tests.DemoProjects.Shop.Abstractions;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private static readonly ConcurrentDictionary<Guid, TEntity> _store = new();

    public virtual ValueTask<TEntity> CreateAsync(TEntity entity, CancellationToken ct)
    {
        var added = _store.TryAdd(entity.Id, entity);

        if (!added)
        {
            throw new InvalidOperationException($"Entity with Id {entity.Id} already exists.");
        }

        return ValueTask.FromResult(entity);
    }

    public virtual ValueTask DeleteAsync(Guid id, CancellationToken ct)
    {
        _store.TryRemove(id, out _);
        return ValueTask.CompletedTask;
    }

    public ValueTask<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
    {
        return ValueTask.FromResult(_store.Values.AsEnumerable());
    }

    public ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return ValueTask.FromResult(_store.TryGetValue(id, out var entity) ? entity : null);
    }

    public ValueTask<TEntity> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        _store[entity.Id] = entity;
        return ValueTask.FromResult(entity);
    }
}