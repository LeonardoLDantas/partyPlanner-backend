using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Extensions;

public static class AuthMappingExtensions
{
    public static AuthenticatedUserResponse ToAuthenticatedUserResponse(this User user)
    {
        return new AuthenticatedUserResponse(user.Id, user.Name, user.Email);
    }
}
