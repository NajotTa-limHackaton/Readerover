using Microsoft.EntityFrameworkCore;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Extensions.Querying;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public ValueTask<Category> CreateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return categoryRepository.CreateAsync(category, saveChanges, cancellationToken);
    }

    public ValueTask<Category?> DeleteAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return categoryRepository.DeleteAsync(category, saveChanges, cancellationToken);
    }

    public ValueTask<Category?> DeleteByIdAsync(Guid categoryId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return categoryRepository.DeleteByIdAsync(categoryId, saveChanges, cancellationToken);
    }

    public IQueryable<Category> Get(FilterPagination? filterPagination = default, Expression<Func<Category, bool>>? predicate = null, bool asNoTracking = false)
    {
        return filterPagination is not null ? categoryRepository.Get(predicate, asNoTracking).ApplyPagination(filterPagination) : categoryRepository.Get(predicate, asNoTracking);
    }

    public ValueTask<Category?> GetByIdAsync(Guid categoryId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return categoryRepository.GetByIdAsync(categoryId, asNoTracking, cancellationToken);
    }

    public ValueTask<Category> UpdateAsync(Category category, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return categoryRepository.UpdateAsync(category, saveChanges, cancellationToken);
    }
}
