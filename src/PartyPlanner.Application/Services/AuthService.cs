using PartyPlanner.Application.Interface;
using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Extensions;

namespace PartyPlanner.Application.Services;

public sealed class AuthService(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IGoogleTokenVerifier googleTokenVerifier) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existingUser = await authRepository.GetUserByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Ja existe uma conta cadastrada com esse e-mail.");
        }

        var user = new User(
            Guid.NewGuid(),
            string.IsNullOrWhiteSpace(request.Name) ? "Usuario Party Planner" : request.Name.Trim(),
            email,
            passwordHasher.Hash(request.Password),
            true
        );

        await authRepository.AddUserAsync(user, cancellationToken);
        await authRepository.SaveChangesAsync(cancellationToken);

        return tokenService.Create(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await authRepository.GetUserByEmailAsync(email, cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Usuario ou senha invalidos.");
        }

        return tokenService.Create(user);
    }

    public async Task<AuthResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken cancellationToken = default)
    {
        var googleUser = await googleTokenVerifier.VerifyAsync(request.IdToken, cancellationToken);

        var user = await authRepository.GetUserByExternalLoginAsync("Google", googleUser.Subject, cancellationToken)
            ?? await authRepository.GetUserByEmailAsync(googleUser.Email.Trim().ToLowerInvariant(), cancellationToken);

        if (user is null)
        {
            user = new User(
                Guid.NewGuid(),
                string.IsNullOrWhiteSpace(googleUser.Name) ? "Usuario Google" : googleUser.Name.Trim(),
                googleUser.Email.Trim().ToLowerInvariant(),
                string.Empty,
                googleUser.EmailVerified
            );

            user.AddExternalLogin(new UserExternalLogin(
                Guid.NewGuid(),
                user.Id,
                "Google",
                googleUser.Subject,
                user.Email
            ));

            await authRepository.AddUserAsync(user, cancellationToken);
        }
        else
        {
            user.AddExternalLogin(new UserExternalLogin(
                Guid.NewGuid(),
                user.Id,
                "Google",
                googleUser.Subject,
                user.Email
            ));

            if (googleUser.EmailVerified)
            {
                user.ConfirmEmail();
            }
        }

        await authRepository.SaveChangesAsync(cancellationToken);
        return tokenService.Create(user);
    }

    public async Task<AuthenticatedUserResponse?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await authRepository.GetUserByIdAsync(userId, cancellationToken);
        return user?.ToAuthenticatedUserResponse();
    }
}
