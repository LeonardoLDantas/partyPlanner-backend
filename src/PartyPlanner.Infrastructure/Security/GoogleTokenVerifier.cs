using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using PartyPlanner.Application.Interfaces;

namespace PartyPlanner.Infrastructure.Security;

public sealed class GoogleTokenVerifier(IConfiguration configuration) : IGoogleTokenVerifier
{
    public async Task<GoogleUserInfo> VerifyAsync(string idToken, CancellationToken cancellationToken = default)
    {
        var clientId = configuration["GoogleAuth:ClientId"];
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException("GoogleAuth:ClientId nao foi configurado no backend.");
        }

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [clientId]
        });

        return new GoogleUserInfo(
            payload.Subject,
            payload.Email,
            payload.Name,
            payload.EmailVerified);
    }
}
