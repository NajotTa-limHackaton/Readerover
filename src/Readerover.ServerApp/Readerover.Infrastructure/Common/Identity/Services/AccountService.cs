using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Extensions.Querying;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class AccountService(
    IUserService userService,
    IPasswordHasherService passwordHasher,
    IWebHostEnvironment environment,
    IFileValidatorService fileValidatorService) : IAccountService
{
    public async ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (user.Password.Length < 8) throw new ArgumentException("invalid Password");

        user.Password = passwordHasher.Hash(user.Password);

        await userService.CreateAsync(user, saveChanges, cancellationToken);

        return user;
    }

    public ValueTask<User?> DeleteAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userService.DeleteAsync(user, saveChanges, cancellationToken);
    }

    public ValueTask<User?> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userService.DeleteByIdAsync(id, saveChanges, cancellationToken);
    }

    public IQueryable<User> Get(FilterPagination? filterPagination = default, bool applyPagination = true, Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false)
    {
        var users = userService.Get(predicate, asNoTracking);

        if (applyPagination) users = users.ApplyPagination(filterPagination);

        return users;
    }

    public ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return userService.GetByIdAsync(id, asNoTracking, cancellationToken);
    }

    public ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userService.UpdateAsync(user, saveChanges, cancellationToken);
    }

    public async ValueTask<string> UploadImageAsync(IFormFile file, Guid userId, CancellationToken cancellationToken = default)
    {
        await fileValidatorService.ValidateProfileImage(file, cancellationToken);

        var fileExtension = file.FileName.Split('.').LastOrDefault() ?? throw new InvalidOperationException("Invalid file extension");
        var usersPath = Path.Combine(environment.WebRootPath, "users");
        var imagePath = Path.Combine(usersPath, $"{userId}.{fileExtension}");
        if (File.Exists(imagePath)) File.Delete(imagePath);

        if (!Directory.Exists(usersPath)) Directory.CreateDirectory(usersPath);

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return imagePath;
    }
}
