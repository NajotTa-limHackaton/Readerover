using FluentValidation;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
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

        var updatedUser = await userRepository.UpdateAsync(user, saveChanges, cancellationToken);

        return updatedUser;
    }

    public async ValueTask<User> UpdateImagePath(Guid userId, string imagePath, CancellationToken cancellationToken = default)
    {
        var foundUser = await GetByIdAsync(userId, cancellationToken: cancellationToken)
            ?? throw new EntityNotFoundException("User not found with this id!");

        foundUser.ImageUrl = imagePath;

        var updatedUser = await userRepository.UpdateAsync(foundUser, cancellationToken: cancellationToken);

        return updatedUser;
    }
}
