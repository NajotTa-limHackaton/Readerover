using Microsoft.Extensions.Options;
using Readerover.Application.Common.Identity.Services;
using Readerover.Infrastructure.Common.Extensions;
using Readerover.Infrastructure.Common.Settings;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class UrlService(IOptions<UrlSettings> options) : IUrlService
{
    private readonly UrlSettings _urlSettings = options.Value;

    public ValueTask<string> GetAbsoluteUrlFromRelativePath(string url, CancellationToken cancellationToken = default)
    {
        return new(Path.Combine(_urlSettings.BaseUrl, url.ToUrl()));
    }
}
