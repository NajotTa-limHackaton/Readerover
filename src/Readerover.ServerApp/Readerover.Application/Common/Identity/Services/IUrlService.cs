namespace Readerover.Application.Common.Identity.Services;

public interface IUrlService
{
    ValueTask<string> GetAbsoluteUrlFromRelativePath(string url, CancellationToken cancellationToken = default);
}
