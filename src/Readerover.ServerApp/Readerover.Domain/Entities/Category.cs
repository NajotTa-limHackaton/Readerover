using Readerover.Domain.Common.Models;

namespace Readerover.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public List<SubCategory> SubCategories { get; set; } = new();
}
