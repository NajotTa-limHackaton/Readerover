using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Entities;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public ValueTask<Book> CreateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if(book.Name.Length < 3)
            throw new ArgumentException("book name is short");

        return bookRepository.CreateAsync(book, saveChanges, cancellationToken);
    }

    public ValueTask<Book?> DeleteAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return bookRepository.DeleteAsync(book, saveChanges, cancellationToken);
    }

    public ValueTask<Book?> DeleteByIdAsync(Guid bookId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return bookRepository.DeleteByIdAsync(bookId, saveChanges, cancellationToken);
    }

    public IQueryable<Book> Get(Expression<Func<Book, bool>>? predicate = null, bool asNoTracking = false)
    {
        return bookRepository.Get(predicate, asNoTracking);
    }

    public ValueTask<Book?> GetByIdAsync(Guid bookId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return bookRepository.GetByIdAsync(bookId, asNoTracking, cancellationToken);
    }

    public ValueTask<Book> UpdateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return bookRepository.UpdateAsync(book, saveChanges, cancellationToken);
    }
}
