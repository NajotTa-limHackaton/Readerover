namespace Readerover.Api.Common.Dtos;

public class AuthorDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;
}
