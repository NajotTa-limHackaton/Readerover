using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Domain.Exceptions;
using Readerover.Infrastructure.Common.Extensions.Querying;
using System.Linq.Expressions;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class BookOrchestrationService(
    IBookService bookService,
    IFileValidatorService fileValidatorService,
    IWebHostEnvironment environment,
    IUrlService urlService)
    : IBookOrchestrationService
{
    public async ValueTask<string> UploadBookFile(
        Guid bookId,
        IFormFile book,
        CancellationToken cancellationToken = default)
    {
        var foundBook = await bookService.GetByIdAsync(bookId)
            ?? throw new EntityNotFoundException("Book not found!");

        var validationResult = await fileValidatorService.ValidateBookFileAsync(book, cancellationToken);
        var bookPath = Path.Combine(environment.WebRootPath, "books", bookId.ToString());
        if (validationResult) await fileValidatorService.DeleteBookDocumentIfExist(bookPath, cancellationToken);
        if (!Directory.Exists(bookPath)) Directory.CreateDirectory(bookPath);

        var extension = book.FileName.Split('.').Last();
        var relativePath = Path.Combine("books", bookId.ToString(), $"document.{extension}");
        var absolutePath = Path.Combine(environment.WebRootPath, relativePath);

        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
        {
            await book.CopyToAsync(fileStream, cancellationToken);
        }

        var absoluteUrl = await urlService.GetAbsoluteUrlFromRelativePath(relativePath);
        foundBook.BookUrl = absoluteUrl;
        await bookService.UpdateAsync(foundBook);

        return absoluteUrl;
    }

    public async ValueTask<string> UploadBookAudio(
        Guid bookId,
        IFormFile audio,
        CancellationToken cancellationToken = default)
    {
        var foundBook = await bookService.GetByIdAsync(bookId)
           ?? throw new EntityNotFoundException("Book not found!");

        var validationResult = await fileValidatorService.ValidateBookAudioAsync(audio, cancellationToken);
        var bookPath = Path.Combine(environment.WebRootPath, "books", bookId.ToString());
        if (validationResult) await fileValidatorService.DeleteBookAudioFileIfExist(bookPath, cancellationToken);

        var extension = audio.FileName.Split('.').Last();
        var relativePath = Path.Combine("books", bookId.ToString(), $"audio.{extension}");
        var absolutePath = Path.Combine(environment.WebRootPath, relativePath);

        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
        {
            await audio.CopyToAsync(fileStream, cancellationToken);
        }

        var absoluteUrl = await urlService.GetAbsoluteUrlFromRelativePath(relativePath);
        foundBook.AudioUrl = absoluteUrl;
        await bookService.UpdateAsync(foundBook);

        return absoluteUrl;
    }


    public IQueryable<Book> Get(FilterPagination filterPagination, Expression<Func<Book, bool>>? predicate = null, bool asNoTracking = false)
    {
        return bookService.Get(predicate, asNoTracking).ApplyPagination(filterPagination);
    }
}
