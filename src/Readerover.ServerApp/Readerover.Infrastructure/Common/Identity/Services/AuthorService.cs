using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class AuthorService(
    IAuthorRepository authorRepository,
    IFileValidatorService fileValidatorService,
    IWebHostEnvironment environment,
    IUrlService urlService) 
    : IAuthorService
{
    public ValueTask<Author> CreateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return authorRepository.CreateAsync(author,saveChanges, cancellationToken);
    }

    public ValueTask<Author?> DeleteAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return authorRepository.DeleteAsync(author,saveChanges, cancellationToken); 
    }

    public ValueTask<Author?> DeleteByIdAsync(Guid authorId, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return authorRepository.DeleteByIdAsync(authorId,saveChanges, cancellationToken);
    }

    public IQueryable<Author> Get(Expression<Func<Author, bool>>? predicate = null, bool asNoTracking = false)
    {
        return authorRepository.Get(predicate, asNoTracking);   
    }

    public ValueTask<Author?> GetByIdAsync(Guid authorId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return authorRepository.GetByIdAsync(authorId, asNoTracking, cancellationToken);
    }

    public ValueTask<Author> UpdateAsync(Author author, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return authorRepository.UpdateAsync(author,saveChanges, cancellationToken);
    }

    public async ValueTask<string> UploadImageAsync(Guid authorId, IFormFile formFile, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var author = await authorRepository.GetByIdAsync(authorId, cancellationToken:cancellationToken)
            ?? throw new EntityNotFoundException("Author not found for upload image!");

        var validationResult = await fileValidatorService.ValidateImageAsync(formFile, cancellationToken);

        if (validationResult) await fileValidatorService.DeleteIfImageExistsAsync(authorId,"authors", environment.WebRootPath, cancellationToken);
        var extension = formFile.FileName.Split('.').Last();


        var relativePath = Path.Combine(environment.WebRootPath, "authors");
        if (!Directory.Exists(relativePath)) Directory.CreateDirectory(relativePath);

        var imageRelativePath = Path.Combine("authors", $"{authorId}.{extension}");
        var absolutePath = Path.Combine(environment.WebRootPath, imageRelativePath);

        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream, cancellationToken);
        }

        var absoluteUrl = await urlService.GetAbsoluteUrlFromRelativePath(imageRelativePath, cancellationToken);
        author.ImageUrl = absoluteUrl;
        await UpdateAsync(author, cancellationToken:cancellationToken);

        return absoluteUrl;
    }
}
