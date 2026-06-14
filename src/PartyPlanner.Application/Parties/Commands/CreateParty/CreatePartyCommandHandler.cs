using MediatR;
using PartyPlanner.Application.DTOs.Responses;
using PartyPlanner.Application.Mappings;
using PartyPlanner.Application.Parties.Events;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Enums;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;
using System.Globalization;

namespace PartyPlanner.Application.Parties.Commands.CreateParty;

public sealed class CreatePartyCommandHandler(
    IPartyRepository partyRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPublisher publisher) : IRequestHandler<CreatePartyCommand, PartyResponse>
{
    public async Task<PartyResponse> Handle(CreatePartyCommand request, CancellationToken cancellationToken)
    {
        EnsureScheduleIsCurrentOrFuture(request.Date, request.Time);

        var party = new EntityParty(
            Guid.NewGuid(),
            request.OwnerUserId,
            request.Name.Trim(),
            request.Category ?? PartyCategory.Outros,
            string.IsNullOrWhiteSpace(request.Date) ? "Data a definir" : request.Date.Trim(),
            string.IsNullOrWhiteSpace(request.Time) ? "19:00" : request.Time.Trim(),
            string.IsNullOrWhiteSpace(request.Location) ? "Local a definir" : request.Location.Trim(),
            string.IsNullOrWhiteSpace(request.CoverImageUrl) ? string.Empty : request.CoverImageUrl.Trim(),
            Math.Max(request.ExpectedGuests ?? 0, 0),
            new EntityBudget(request.EstimatedBudget, 0, []),
            request.IsFinalized ?? false
        );

        await partyRepository.AddAsync(party, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await publisher.Publish(new PartyCreatedEvent(party.OwnerUserId, party.Name), cancellationToken);

        return party.ToResponse();
    }

    private void EnsureScheduleIsCurrentOrFuture(string? date, string? time)
    {
        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var eventDate)
            || !TimeOnly.TryParse(time, CultureInfo.InvariantCulture, DateTimeStyles.None, out var eventTime))
        {
            return;
        }

        var scheduledAt = eventDate.ToDateTime(eventTime);
        if (scheduledAt < dateTimeProvider.Now)
        {
            throw new InvalidOperationException("Informe uma data e um horario atuais ou futuros para a festa.");
        }
    }
}
