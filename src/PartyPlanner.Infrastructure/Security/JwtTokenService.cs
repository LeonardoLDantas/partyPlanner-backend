using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Extensions;

namespace PartyPlanner.Infrastructure.Security;

public sealed class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public AuthResponse Create(User user)
    {
        var issuer = configuration["Jwt:Issuer"] ?? "PartyPlanner.Api";
        var audience = configuration["Jwt:Audience"] ?? "PartyPlanner.Mobile";
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key nao foi configurada.");
        var expiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var parsedMinutes)
            ? parsedMinutes
            : 120;

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAtUtc,
            user.ToAuthenticatedUserResponse());
    }
}
