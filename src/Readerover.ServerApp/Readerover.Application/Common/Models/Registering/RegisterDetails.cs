using System.Globalization;

namespace Readerover.Application.Common.Models.Registering;

public class RegisterDetails
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string Password { get; set; } = default!;

    public DateTime BirthDate { get; set; }

    public string Country { get; set; } = default!;
}
