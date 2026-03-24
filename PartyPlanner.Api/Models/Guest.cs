namespace PartyPlanner.Api.Models;

public sealed class Guest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
