using PartyPlanner.Core.DTO.Requests;
using PartyPlanner.Core.DTO.Responses;

namespace PartyPlanner.Application.Interface;

public interface IPartyService
{
    Task<IReadOnlyCollection<PartyResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PartyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PartyResponse> CreateAsync(CreatePartyRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddTaskAsync(Guid partyId, CreateTaskRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> ToggleTaskAsync(Guid partyId, Guid taskId, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddGuestAsync(Guid partyId, CreateGuestRequest request, CancellationToken cancellationToken = default);
    Task<PartyResponse?> AddBudgetItemAsync(Guid partyId, CreateBudgetItemRequest request, CancellationToken cancellationToken = default);
}
