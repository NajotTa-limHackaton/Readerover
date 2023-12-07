using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

public class CategoryRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<Category, AppDbContext>(dbContext, cacheBroker), ICategoryRepository
{
    public new ValueTask<Category> CreateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(category, saveChanges, cancellationToken);
    }

    public new ValueTask<Category?> DeleteAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(category, saveChanges, cancellationToken);
    }

    public new ValueTask<Category?> DeleteByIdAsync(Guid categoryId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteByIdAsync(categoryId, saveChanges, cancellationToken);
    }

    public new IQueryable<Category> Get(Expression<Func<Category, bool>>? predicate = null, bool asNoTracking = false)
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<Category?> GetByIdAsync(Guid categoryId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return base.GetByIdAsync(categoryId, asNoTracking, cancellationToken);
    }

    public new ValueTask<Category> UpdateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundCategory = dbContext.Categories.SingleOrDefault(dbCategory => dbCategory.Id == category.Id)
            ?? throw new EntityNotFoundException("category not found with this Id");

        foundCategory.Name = category.Name;
        foundCategory.IsActive = category.IsActive;

        return base.UpdateAsync(foundCategory, saveChanges, cancellationToken);
    }
}
