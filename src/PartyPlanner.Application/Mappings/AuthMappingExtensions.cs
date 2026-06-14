using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Mappings;

public static class AuthMappingExtensions
{
    public static AuthenticatedUserResponse ToAuthenticatedUserResponse(this EntityUser user)
    {
        return new AuthenticatedUserResponse(user.Id, user.Name, user.Email);
    }
}
