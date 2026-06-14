using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Exceptions;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await authRepository.GetUserByEmailAsync(email, cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        return tokenService.Create(user);
    }
}
