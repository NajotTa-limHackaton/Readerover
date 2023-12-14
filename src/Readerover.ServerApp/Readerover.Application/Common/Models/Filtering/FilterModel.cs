using Readerover.Application.Common.enums;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;

namespace Readerover.Application.Common.Models.Filtering;

public class FilterModel : FilterPagination
{
    public List<Category> Categories { get; set; } = new();

    public List<SubCategory> SubCategories { get; set; } = new();

    public Author Author { get; set; } = new();

    public OrderBy OrderBy { get; set; }
}
