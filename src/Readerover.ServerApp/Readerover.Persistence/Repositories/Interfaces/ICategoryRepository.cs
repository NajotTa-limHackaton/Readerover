using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories.Interfaces;

public interface ICategoryRepository
{
    IQueryable<Category> Get(Expression<Func<Category, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<Category?> GetByIdAsync(Guid categoryId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Category> CreateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Category> UpdateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Category?> DeleteByIdAsync(Guid categoryId, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Category?> DeleteAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default);
}
