namespace PartyPlanner.Api.Models;

public sealed class PartyTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public bool Done { get; set; }
}
