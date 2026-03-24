namespace PartyPlanner.Api.Models;

public sealed class Party
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<PartyTask> Tasks { get; set; } = [];
    public List<Guest> Guests { get; set; } = [];
    public Budget Budget { get; set; } = new();
}
