using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Application.Interfaces;

public interface ITokenService
{
    AuthResponse Create(EntityUser user);
}
