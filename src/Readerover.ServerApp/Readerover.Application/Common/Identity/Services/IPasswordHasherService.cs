namespace Readerover.Application.Common.Identity.Services;

public interface IPasswordHasherService
{
    string Hash(string password);

    bool Verify(string password, string hashPassword);
}
