namespace PartyPlanner.Core.Exceptions;
public sealed class UserAlreadyExistsException(string email) 
    : DomainException($"Ja existe uma conta cadastrada com o e-mail {email}.");
