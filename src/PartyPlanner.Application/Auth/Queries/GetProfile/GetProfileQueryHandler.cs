using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Queries.GetProfile;

public sealed class GetProfileQueryHandler(
    IAuthRepository authRepository) : IRequestHandler<GetProfileQuery, AuthenticatedUserResponse?>
{
    public async Task<AuthenticatedUserResponse?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await authRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        return user?.ToAuthenticatedUserResponse();
    }
}
