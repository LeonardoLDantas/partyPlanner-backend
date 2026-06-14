using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
