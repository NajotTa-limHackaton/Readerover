using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Application.Common.Identity.Services;

public interface IBookOrchestrationService
{
    IQueryable<Book> Get(
        FilterPagination filterPagination,
        Expression<Func<Book, bool>>? predicate = default,
        bool asNoTracking = false);

    ValueTask<string> UploadBookFile(
        Guid bookId,
        IFormFile book,
        CancellationToken cancellationToken = default);

    ValueTask<string> UploadBookAudio(
        Guid bookId,
        IFormFile audio,
        CancellationToken cancellationToken = default);
}