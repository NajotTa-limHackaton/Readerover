using FluentValidation;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Entities;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class UserService(IUserRepository userRepository, IValidator<User> validator) : IUserService
{
    public async ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(user);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await userRepository.CreateAsync(user, saveChanges, cancellationToken);

        return user;
    }

    public ValueTask<User?> DeleteAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userRepository.DeleteAsync(user, cancellationToken:cancellationToken);
    }

    public ValueTask<User?> DeleteByIdAsync(Guid userId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userRepository.DeleteByIdAsync(userId, saveChanges, cancellationToken);
    }

    public IQueryable<User> Get(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false)
    {
        return userRepository.Get(predicate, asNoTracking);
    }

    public ValueTask<User?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(userId, asNoTracking, cancellationToken);
    }

    public async ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(user);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await userRepository.UpdateAsync(user, saveChanges, cancellationToken);

        return user;
    }
}
