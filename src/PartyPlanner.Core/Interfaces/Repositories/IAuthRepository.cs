using PartyPlanner.Core.Entities;

namespace PartyPlanner.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<EntityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<EntityUser?> GetUserByExternalLoginAsync(string provider, string providerUserId, CancellationToken cancellationToken = default);
    Task<EntityUser?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddUserAsync(EntityUser user, CancellationToken cancellationToken = default);
    Task AddPasswordResetTokenAsync(EntityPasswordResetToken token, CancellationToken cancellationToken = default);
    Task<EntityPasswordResetToken?> GetPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default);
}
