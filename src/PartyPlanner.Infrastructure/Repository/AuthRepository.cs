using Microsoft.EntityFrameworkCore;
using PartyPlanner.Application.Interface;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data;

namespace PartyPlanner.Infrastructure.Repository;

public sealed class AuthRepository(PartyPlannerDbContext dbContext) : IAuthRepository
{
    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public Task<User?> GetUserByExternalLoginAsync(string provider, string providerUserId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(
                user => user.ExternalLogins.Any(login =>
                    login.Provider == provider &&
                    login.ProviderUserId == providerUserId),
                cancellationToken);
    }

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.ExternalLogins)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
