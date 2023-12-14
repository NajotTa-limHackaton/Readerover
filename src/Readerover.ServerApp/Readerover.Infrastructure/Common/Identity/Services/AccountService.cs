using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Infrastructure.Common.Extensions;
using Readerover.Infrastructure.Common.Extensions.Querying;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class AccountService(
    IUserService userService,
    IPasswordHasherService passwordHasher,
    IWebHostEnvironment environment,
    IFileValidatorService fileValidatorService,
    IUrlService urlService) : IAccountService
{
    public async ValueTask<bool> AddBookToSavedAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var found = await GetByIdAsync(userId, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("User not found wirh this id!");

        found.SavedBooks.Add(bookId);
        await userService.UpdateAsync(found, cancellationToken: cancellationToken);

        return true;
    }

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
        var user = await GetByIdAsync(userId)
            ?? throw new EntityNotFoundException("User not found!");

        var validationResult = await fileValidatorService.ValidateImageAsync(file, cancellationToken);
        if (validationResult) await fileValidatorService.DeleteIfImageExistsAsync(userId, "users", environment.WebRootPath, cancellationToken);

        var fileExtension = file.FileName.Split('.').LastOrDefault() ?? throw new InvalidOperationException("Invalid file extension");

        var usersFolderName = "users";
        var usersFolderPath = Path.Combine(environment.WebRootPath, usersFolderName);
        if (!Directory.Exists(usersFolderPath)) Directory.CreateDirectory(usersFolderPath);
        
        var relativePath = Path.Combine(usersFolderName, $"{userId}.{fileExtension}");
        var absolutePath = Path.Combine(environment.WebRootPath, relativePath);


        using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var absoluteUrl = await urlService.GetAbsoluteUrlFromRelativePath(relativePath);
        user.ImageUrl = absoluteUrl;
        await UpdateAsync(user, cancellationToken: cancellationToken);

        return absoluteUrl;
    }
}
