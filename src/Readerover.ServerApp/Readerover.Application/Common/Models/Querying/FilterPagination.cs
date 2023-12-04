namespace Readerover.Application.Common.Models.Querying;

public class FilterPagination
{
    public int PageToken { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
