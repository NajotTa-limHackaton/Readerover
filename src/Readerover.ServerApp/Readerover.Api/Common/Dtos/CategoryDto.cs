namespace Readerover.Api.Common.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }
}
