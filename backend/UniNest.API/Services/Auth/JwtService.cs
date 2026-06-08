using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UniNest.Api.Services.Auth;
using UniNest.API.Configurations;
using UniNest.API.Models.User;

namespace UniNest.API.Services.Auth;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.Id),
            new Claim(JwtRegisteredClaimNames.Email,user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}