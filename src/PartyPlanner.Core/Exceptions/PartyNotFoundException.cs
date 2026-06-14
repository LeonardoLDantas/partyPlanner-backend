namespace PartyPlanner.Core.Exceptions;
public sealed class PartyNotFoundException(Guid partyId) 
    : DomainException($"Festa com ID {partyId} nao encontrada.");
