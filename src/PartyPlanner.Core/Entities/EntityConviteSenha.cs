using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlanner.Core.Entities;

[Table("ConviteSenhas")]
public sealed class EntityConviteSenha
{
    private EntityConviteSenha() { }

    public EntityConviteSenha(Guid id, string codigo)
    {
        Id = id;
        Codigo = codigo;
    }

    public Guid Id { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
}
