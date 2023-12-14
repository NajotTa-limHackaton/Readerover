using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Extensions.Querying;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class SubCategoryService(ISubCategoryRepository subCategoryRepository) : ISubCategoryService
{
    public ValueTask<SubCategory> CreateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return subCategoryRepository.CreateAsync(subCategory, saveChanges, cancellationToken);
    }

    public ValueTask<SubCategory?> DeleteAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return subCategoryRepository.DeleteAsync(subCategory, saveChanges, cancellationToken);
    }

    public ValueTask<SubCategory?> DeleteByIdAsync(Guid subCategoryId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return subCategoryRepository.DeleteByIdAsync(subCategoryId, saveChanges, cancellationToken);
    }

    public IQueryable<SubCategory> Get(FilterPagination? filterPagination = default, Expression<Func<SubCategory, bool>>? predicate = null, bool asNoTracking = false)
    {
        return filterPagination is not null 
            ? subCategoryRepository.Get(predicate, asNoTracking).ApplyPagination(filterPagination)
            : subCategoryRepository.Get(predicate, asNoTracking);
    }

    public ValueTask<SubCategory?> GetByIdAsync(Guid subCategoryId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return subCategoryRepository.GetByIdAsync(subCategoryId, asNoTracking, cancellationToken);
    }

    public ValueTask<SubCategory> UpdateAsync(SubCategory subCategory, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return subCategoryRepository.UpdateAsync(subCategory, saveChanges, cancellationToken);
    }
}
