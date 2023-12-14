using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

public class BookRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<Book, AppDbContext>(dbContext, cacheBroker), IBookRepository
{
    public new ValueTask<Book> CreateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(book, saveChanges, cancellationToken);
    }

    public new ValueTask<Book?> DeleteAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(book, saveChanges, cancellationToken);
    }

    public new ValueTask<Book?> DeleteByIdAsync(Guid bookId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return DeleteByIdAsync(bookId, saveChanges, cancellationToken);
    }

    public new IQueryable<Book> Get(Expression<Func<Book, bool>>? predicate = null, bool asNoTracking = false)
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<Book?> GetByIdAsync(Guid bookId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return base.GetByIdAsync(bookId, asNoTracking, cancellationToken);
    }

    public new ValueTask<Book> UpdateAsync(Book book, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var found = dbContext.Books.SingleOrDefault(dbBook => dbBook.Id == book.Id)
            ?? throw new EntityNotFoundException("Book not found with this Id!");

        found.Name = book.Name;
        found.CategoryId = book.CategoryId;
        found.SubCategory = book.SubCategory;
        found.AuthorId = book.AuthorId;
        found.BookUrl = book.BookUrl;
        found.AudioUrl = book.AudioUrl;

        return base.UpdateAsync(found, saveChanges, cancellationToken);
    }
}
