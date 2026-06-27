using System.ComponentModel.DataAnnotations.Schema;
using PartyPlanner.Core.Enums;

namespace PartyPlanner.Core.Entities;

[Table("Convites")]
public sealed class EntityConvite
{
    private EntityConvite() { }

    public EntityConvite(Guid id, string nome, string? observacao, InviteType tipo, string? senhaPresente)
    {
        Id = id;
        Nome = nome;
        Observacao = observacao ?? string.Empty;
        Tipo = tipo;
        SenhaPresente = senhaPresente ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Observacao { get; private set; } = string.Empty;
    public InviteType Tipo { get; private set; }
    public string SenhaPresente { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public List<EntityConviteSenha> Senhas { get; private set; } = [];
    public List<EntityGuest> Guests { get; private set; } = [];

    public void AddSenha(EntityConviteSenha senha) => Senhas.Add(senha);
    public void AddGuest(EntityGuest guest) => Guests.Insert(0, guest);

    public void Update(string nome, string? observacao, InviteType tipo, string? senhaPresente)
    {
        Nome = nome;
        Observacao = observacao ?? string.Empty;
        Tipo = tipo;
        SenhaPresente = senhaPresente ?? string.Empty;
    }
}
