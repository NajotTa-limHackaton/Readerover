using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Readerover.Application.Common.Identity.Services;
using Readerover.Domain.Common.Constants;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Readerover.Infrastructure.Common.Identity.Services;

public class AccessTokenGeneratorService(IOptions<JwtSettings> jwtSettings) : IAccessTokenGeneratorService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string GetToken(User user)
    {
        var jwtToken = GetJwtToken(user);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    private JwtSecurityToken GetJwtToken(User user)
    {
        var claims = GetClaims(user);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer:_jwtSettings.ValidIssuer,
            audience:_jwtSettings.ValidAudience,
            claims:claims,
            notBefore:DateTime.UtcNow,
            expires:DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            signingCredentials:credentials);
    }

    private List<Claim> GetClaims(User user)
    {
        return new List<Claim>
        {
            new(ClaimConstants.UserId, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.EmailAddress)
        };
    }
}
