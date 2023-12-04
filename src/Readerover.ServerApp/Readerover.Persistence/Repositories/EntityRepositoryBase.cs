using Microsoft.EntityFrameworkCore;
using Readerover.Domain.Common.Models;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

/// <summary>
/// biznes logika va database o'rtasidagi abstracktsiya
/// Hamma typelar bilan ishlay olishi uchun generic qilingan
/// bazaga requestlarni kamaytirish va query tezligini oshirish uchun  Cache ham ishlatilgan
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TContext"></typeparam>
/// <param name="dbContext"></param>
/// <param name="cacheBroker"></param>
public class EntityRepositoryBase<TEntity, TContext>(TContext dbContext, ICacheBroker cacheBroker)
    where TEntity : class, IEntity
    where TContext : DbContext
{
    protected TContext DbContext => dbContext;

    /// <summary>
    /// predicate qabul qilib o'sha bo'yicha entitylarni olish
    /// </summary>
    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = default, bool asNoTracking = false)
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
            initialQuery = initialQuery.Where(predicate);

        if (asNoTracking)
            initialQuery = initialQuery.AsNoTracking();

        return initialQuery;
    }

    /// <summary>
    /// Id orqali entity olish
    /// </summary>
    /// <exception cref="EntityNotFoundException"></exception>
    protected async ValueTask<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await cacheBroker.GetOrSetAsync<TEntity>(id.ToString(), async () =>
        {
            var initialQuery = DbContext.Set<TEntity>().AsQueryable();

            if (asNoTracking)
                initialQuery = initialQuery.AsNoTracking();

            return await initialQuery.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException("Entity not found! " + nameof(TEntity));
        });
    }

    /// <summary>
    /// Bir nechta Id lar orqali get qilish
    /// </summary>
    /// <returns></returns>
    protected async ValueTask<IList<TEntity>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        var initialQuery = DbContext.Set<TEntity>().AsQueryable();

        if (asNoTracking) initialQuery = initialQuery.AsNoTracking();

        initialQuery = initialQuery.Where(entity => ids.Contains(entity.Id));

        return await initialQuery.ToListAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Entity yaratish
    /// </summary>
    protected async ValueTask<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);

        await cacheBroker.SetAsync(entity.Id.ToString(), entity);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <summary>
    /// Entityni yangilash
    /// </summary>
    protected async ValueTask<TEntity> UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);

        await cacheBroker.SetAsync(entity.Id.ToString(), entity);

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <summary>
    /// entityni o'chirib tashlash
    /// </summary>
    protected async ValueTask<TEntity?> DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);

        await cacheBroker.DeleteAsync(entity.Id.ToString());

        if (saveChanges) await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <summary>
    /// Id orqali entity ni o'chirib tashlash
    /// </summary>
    /// <exception cref="EntityNotFoundException"></exception>
    protected async ValueTask<TEntity?> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var entity = await DbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken) ??
                     throw new EntityNotFoundException($"{nameof(TEntity)} - not found!");

        DbContext.Set<TEntity>().Remove(entity);

        await cacheBroker.DeleteAsync(entity.Id.ToString());

        if (saveChanges)
            await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
