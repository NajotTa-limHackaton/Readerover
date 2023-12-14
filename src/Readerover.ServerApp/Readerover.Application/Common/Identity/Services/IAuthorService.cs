using Microsoft.AspNetCore.Http;
using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Application.Common.Identity.Services;

public interface IAuthorService
{
    IQueryable<Author> Get(Expression<Func<Author, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<Author?> GetByIdAsync(Guid authorId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Author> CreateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Author> UpdateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Author?> DeleteByIdAsync(Guid authorId, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Author?> DeleteAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<string> UploadImageAsync(Guid authorId, IFormFile formFile, bool saveChanges = true, CancellationToken cancellationToken = default);    
}
