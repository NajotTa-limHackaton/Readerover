using Readerover.Application.Common.enums;
using Readerover.Domain.Entities;

namespace Readerover.Application.Common.Models.Filtering;

public class FilterDataModel
{
    public List<Category> Categories { get; set; } = new();

    public List<SubCategory> SubCategories { get; set; } = new();

    public List<Author> Authors { get; set; } = new();

    public OrderBy OrderBy { get; set; }
}
