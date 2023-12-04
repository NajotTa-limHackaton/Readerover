using Readerover.Application.Common.Identity.Services;
using BC = BCrypt.Net.BCrypt;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string Hash(string password) => BC.HashPassword(password);

    public bool Verify(string password, string hashPassword) => BC.Verify(password, hashPassword);
}
