namespace PartyPlanner.Application.Interfaces;

public interface IGoogleTokenVerifier
{
    Task<GoogleUserInfo> VerifyAsync(string idToken, CancellationToken cancellationToken = default);
}
