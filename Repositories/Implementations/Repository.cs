using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSite3.Models;
using PSite3.Repositories.Interfaces;

namespace PSite3.Repositories.Implementations;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ShopContext Context;
    protected readonly DbSet<TEntity> Entities;

    public Repository(ShopContext context)
    {
        Context = context;
        Entities = context.Set<TEntity>();
    }

    protected IQueryable<TEntity> Query() => Entities.AsQueryable();

    public virtual Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => Entities.FindAsync(new object?[] { id }, cancellationToken).AsTask();

    public virtual Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Query().AnyAsync(predicate, cancellationToken);

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
        => Query().CountAsync(cancellationToken);

    public virtual Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Query().CountAsync(predicate, cancellationToken);

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => Entities.AddAsync(entity, cancellationToken).AsTask();

    public virtual void Update(TEntity entity) => Entities.Update(entity);

    public virtual void Remove(TEntity entity) => Entities.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);
}
