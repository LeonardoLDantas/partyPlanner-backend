using Microsoft.EntityFrameworkCore;
using PartyPlanner.Core.Interfaces.Repositories;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data;

namespace PartyPlanner.Infrastructure.Repository;

public sealed class AuthRepository(PartyPlannerDbContext dbContext) : IAuthRepository
{
    public Task<EntityUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public Task<EntityUser?> GetUserByExternalLoginAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(
                user => user.ExternalLogins.Any(login =>
                    login.Provider == provider &&
                    login.ProviderUserId == providerUserId),
                cancellationToken);
    }

    public Task<EntityUser?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public Task AddUserAsync(EntityUser user, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AddAsync(user, cancellationToken).AsTask();
    }
}
