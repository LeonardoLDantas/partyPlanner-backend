using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;

namespace PartyPlanner.Application.Interface;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthenticatedUserResponse?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
}
