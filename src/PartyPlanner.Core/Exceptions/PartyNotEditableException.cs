namespace PartyPlanner.Core.Exceptions;
public sealed class PartyNotEditableException(string message) : DomainException(message);
