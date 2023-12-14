using Readerover.Domain.Common.Enums;
using Readerover.Domain.Common.Models;
using System.Text.Json.Serialization;

namespace Readerover.Domain.Entities;

public class User : AuditableEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string Password { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;

    public string? ImageUrl { get; set; }

    public Role Role { get; set; } = Role.User;

    [JsonIgnore]
    public List<Guid> SavedBooks { get; set; } = new();
}
