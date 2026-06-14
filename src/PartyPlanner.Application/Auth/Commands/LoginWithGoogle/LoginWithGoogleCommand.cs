using MediatR;
using PartyPlanner.Application.DTOs.Responses;

namespace PartyPlanner.Application.Auth.Commands.LoginWithGoogle;

public sealed record LoginWithGoogleCommand(string IdToken) : IRequest<AuthResponse>;
