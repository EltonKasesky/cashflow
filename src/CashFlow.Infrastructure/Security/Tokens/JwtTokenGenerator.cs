using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security.Tokens;

internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private const string NAME = "name";
    private const string USER_IDENTIFIER = "id";
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
    }

    public string Generate(User user)
    {
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(GetClaims(user))
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey GetSecurityKey()
    {
        byte[] key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }

    private List<Claim> GetClaims(User user)
    {
        return new List<Claim>()
        {
            new Claim(NAME, user.Name),
            new Claim(USER_IDENTIFIER, user.UserIdentifier.ToString())
        };
    }
}
