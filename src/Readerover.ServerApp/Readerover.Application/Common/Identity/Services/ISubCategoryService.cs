using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Application.Common.Identity.Services;

public interface ISubCategoryService
{
    IQueryable<SubCategory> Get(Expression<Func<SubCategory, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<SubCategory?> GetByIdAsync(Guid subCategoryId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<SubCategory> CreateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<SubCategory> UpdateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<SubCategory?> DeleteByIdAsync(Guid subCategoryId, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<SubCategory?> DeleteAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default);
}
