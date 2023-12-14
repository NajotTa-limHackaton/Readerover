using Readerover.Domain.Entities;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories.Interfaces;

public interface IBookRepository
{
    IQueryable<Book> Get(Expression<Func<Book, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<Book?> GetByIdAsync(Guid bookId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Book> CreateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Book> UpdateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Book?> DeleteByIdAsync(Guid bookId, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<Book?> DeleteAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default);
}
