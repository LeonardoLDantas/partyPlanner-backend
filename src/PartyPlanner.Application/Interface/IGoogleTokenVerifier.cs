namespace PartyPlanner.Application.Interface;

public interface IGoogleTokenVerifier
{
    Task<GoogleUserInfo> VerifyAsync(string idToken, CancellationToken cancellationToken = default);
}
