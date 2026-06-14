using MediatR;
using PartyPlanner.Application.Auth.Events;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Exceptions;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existingUser = await authRepository.GetUserByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException(email);
        }

        var user = new EntityUser(
            Guid.NewGuid(),
            string.IsNullOrWhiteSpace(request.Name) ? "Usuario EntityParty Planner" : request.Name.Trim(),
            email,
            passwordHasher.Hash(request.Password),
            true
        );

        await authRepository.AddUserAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new UserRegisteredEvent(user.Id), cancellationToken);

        return tokenService.Create(user);
    }
}
