namespace Readerover.Api.Common.Dtos;

public class UserDto
{
    public Guid Id {  get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string Password   { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;

    public string? ImageUrl { get; set; }
}
