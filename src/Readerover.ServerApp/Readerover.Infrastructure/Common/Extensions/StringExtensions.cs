namespace Readerover.Infrastructure.Common.Extensions;

public static class StringExtensions
{
    public static string ToUrl(this string path) => path.Replace("\\", "/");
}
