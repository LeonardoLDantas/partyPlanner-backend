using MediatR;
using PartyPlanner.Application.Interfaces;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand, bool>
{
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetToken = await authRepository.GetPasswordResetTokenAsync(request.Token, cancellationToken);
        if (resetToken is null || !resetToken.IsValid) return false;

        var user = await authRepository.GetUserByIdAsync(resetToken.UserId, cancellationToken);
        if (user is null) return false;

        user.UpdatePasswordHash(passwordHasher.Hash(request.NewPassword));
        resetToken.MarkAsUsed();
        await unitOfWork.CommitAsync(cancellationToken);
        return true;
    }
}
