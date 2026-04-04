using PartyPlanner.Core.DTO.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interface;

public interface ITokenService
{
    AuthResponse Create(User user);
}
