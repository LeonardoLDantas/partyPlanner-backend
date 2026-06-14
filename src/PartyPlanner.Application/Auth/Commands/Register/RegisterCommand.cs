using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Auth.Commands.Register;

public sealed record RegisterCommand(string Name, string Email, string Password) : IRequest<AuthResponse>;
