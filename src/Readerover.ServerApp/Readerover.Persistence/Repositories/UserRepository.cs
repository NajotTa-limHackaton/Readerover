using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Caching;
using Readerover.Persistence.DbContexts;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext, ICacheBroker cacheBroker)
    : EntityRepositoryBase<User, AppDbContext>(dbContext, cacheBroker), IUserRepository
{
    public new ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
        => base.CreateAsync(user, saveChanges, cancellationToken);

    public new ValueTask<User?> DeleteByIdAsync(Guid userId, bool saveChanges = true, CancellationToken cancellationToken = default)
        => base.DeleteByIdAsync(userId, saveChanges, cancellationToken);

    public new IQueryable<User> Get(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false)
        => base.Get(predicate, asNoTracking);

    public new ValueTask<User?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
        => base.GetByIdAsync(userId, asNoTracking, cancellationToken);

    public new ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundUser = dbContext.Users.SingleOrDefault(dbUser => dbUser.Id == user.Id)
            ?? throw new EntityNotFoundException("Entity not found!");

        foundUser.FirstName = user.FirstName;
        foundUser.LastName = user.LastName;
        foundUser.EmailAddress = user.EmailAddress;
        foundUser.Password = user.Password;
        foundUser.BirthDate = user.BirthDate;
        foundUser.Country = user.Country;
        foundUser.ImageUrl =user.ImageUrl;

        return base.UpdateAsync(foundUser, saveChanges, cancellationToken);
    }

    ValueTask<User?> IUserRepository.DeleteAsync(User user, bool saveChanges, CancellationToken cancellationToken)
    {
        return base.DeleteAsync(user, saveChanges, cancellationToken);
    }
}
