using Readerover.Domain.Common.Models;
using System.Text.Json.Serialization;

namespace Readerover.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    [JsonIgnore]
    public virtual List<SubCategory> SubCategories { get; set; } = new();

    [JsonIgnore]
    public virtual List<Book> Books { get; set; } = new();
}
