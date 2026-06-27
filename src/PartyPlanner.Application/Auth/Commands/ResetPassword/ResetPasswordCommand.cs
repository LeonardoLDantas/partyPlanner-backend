using MediatR;

namespace PartyPlanner.Application.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string Token, string NewPassword) : IRequest<bool>;
