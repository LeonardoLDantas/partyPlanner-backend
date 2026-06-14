using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Commands.LoginWithGoogle;

public sealed class LoginWithGoogleCommandHandler(
    IAuthRepository authRepository,
    ITokenService tokenService,
    IGoogleTokenVerifier googleTokenVerifier,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginWithGoogleCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginWithGoogleCommand request, CancellationToken cancellationToken)
    {
        var googleUser = await googleTokenVerifier.VerifyAsync(request.IdToken, cancellationToken);

        var user = await authRepository.GetUserByExternalLoginAsync("Google", googleUser.Subject, cancellationToken)
            ?? await authRepository.GetUserByEmailAsync(googleUser.Email.Trim().ToLowerInvariant(), cancellationToken);

        if (user is null)
        {
            user = new EntityUser(
                Guid.NewGuid(),
                string.IsNullOrWhiteSpace(googleUser.Name) ? "Usuario Google" : googleUser.Name.Trim(),
                googleUser.Email.Trim().ToLowerInvariant(),
                string.Empty,
                googleUser.EmailVerified
            );

            user.AddExternalLogin(new EntityUserExternalLogin(
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
            user.AddExternalLogin(new EntityUserExternalLogin(
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

        await unitOfWork.CommitAsync(cancellationToken);
        return tokenService.Create(user);
    }
}
