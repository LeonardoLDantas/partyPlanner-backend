using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;

namespace PartyPlanner.Application.Interface;

public interface IPartyService
{
    Task<IReadOnlyCollection<PartyResponse>> GetAllAsync(Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<PartyResponse?> GetByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken = default);
    Task<PartyResponse> CreateAsync(Guid ownerUserId, CreatePartyRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> UpdateAsync(Guid ownerUserId, Guid partyId, UpdatePartyRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddTaskAsync(Guid ownerUserId, Guid partyId, CreateTaskRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> ToggleTaskAsync(Guid ownerUserId, Guid partyId, Guid taskId, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddGuestAsync(Guid ownerUserId, Guid partyId, CreateGuestRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddBudgetItemAsync(Guid ownerUserId, Guid partyId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default);
    Task<InvitationResponse?> GetInvitationAsync(string token, CancellationToken cancellationToken = default);
    Task<InvitationResponse?> RespondInvitationAsync(string token, RespondInvitationRequest request, CancellationToken cancellationToken = default);
}
