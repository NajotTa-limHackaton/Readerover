using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

public class SubCategoryRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<SubCategory, AppDbContext>(dbContext, cacheBroker), ISubCategoryRepository
{
    public new ValueTask<SubCategory> CreateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(subCategory, saveChanges, cancellationToken);
    }

    public new ValueTask<SubCategory?> DeleteAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(subCategory, saveChanges, cancellationToken);
    }

    public new ValueTask<SubCategory?> DeleteByIdAsync(Guid subCategoryId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteByIdAsync(subCategoryId, saveChanges, cancellationToken);
    }

    public new IQueryable<SubCategory> Get(Expression<Func<SubCategory, bool>>? predicate = null, bool asNoTracking = false)
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<SubCategory?> GetByIdAsync(Guid subCategoryId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return base.GetByIdAsync(subCategoryId, asNoTracking, cancellationToken);
    }

    public new ValueTask<SubCategory> UpdateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var found = dbContext.SubCategories.SingleOrDefault(sCategory => sCategory.Id == subCategory.Id)
            ?? throw new EntityNotFoundException("SubCategory not found with this Id");

        found.Name = subCategory.Name;
        found.CategoryId = subCategory.CategoryId;

        return base.UpdateAsync(found, saveChanges, cancellationToken);
    }
}
