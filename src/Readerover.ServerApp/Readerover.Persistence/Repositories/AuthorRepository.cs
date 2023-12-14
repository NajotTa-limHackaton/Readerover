using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

public class AuthorRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<Author, AppDbContext>(dbContext, cacheBroker), IAuthorRepository
{
    public new ValueTask<Author> CreateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(author, saveChanges, cancellationToken);
    }

    public new ValueTask<Author?> DeleteAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(author, saveChanges, cancellationToken);
    }

    public new ValueTask<Author?> DeleteByIdAsync(Guid authorId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteByIdAsync(authorId, saveChanges, cancellationToken);
    }

    public new IQueryable<Author> Get(Expression<Func<Author, bool>>? predicate = null, bool asNoTracking = false)
    {
        return base.Get(predicate, asNoTracking);
    }

    public new ValueTask<Author?> GetByIdAsync(Guid authorId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return base.GetByIdAsync(authorId, asNoTracking, cancellationToken);
    }

    public new ValueTask<Author> UpdateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var found = dbContext.Authors.FirstOrDefault(dbAuthor => dbAuthor.Id == author.Id)
            ?? throw new EntityNotFoundException("Author not found with this Id for update!");

        found.FullName = author.FullName;
        found.BirthDate = author.BirthDate;
        found.Country = author.Country;
        found.ImageUrl = author.ImageUrl;

        return base.UpdateAsync(author, saveChanges, cancellationToken);
    }
}
