using Readerover.Domain.Entities;

namespace Readerover.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    /// <summary>
    /// User uchun web token generate qilish
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GetToken(User user);
}
