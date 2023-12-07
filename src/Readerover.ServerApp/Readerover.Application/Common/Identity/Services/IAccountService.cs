using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Application.Common.Identity.Services;

public interface IAccountService
{
    IQueryable<User> Get(FilterPagination? filterPagination = default, bool applyPagination = true, Expression<Func<User, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<User?> DeleteAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<User?> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<string> UploadImageAsync(IFormFile file, Guid userId, CancellationToken cancellationToken = default);
}
