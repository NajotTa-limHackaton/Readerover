using Readerover.Domain.Common.Models;
using System.Text.Json.Serialization;

namespace Readerover.Domain.Entities;

public class Author : AuditableEntity
{
    public string FullName { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;
    
    public string? ImageUrl { get; set; } = default!;

    [JsonIgnore]
    public virtual List<Book> Books { get; set; } = new();
}
