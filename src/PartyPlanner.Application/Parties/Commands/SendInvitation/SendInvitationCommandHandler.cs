using MediatR;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Parties.Commands.SendInvitation;

public sealed class SendInvitationCommandHandler(
    IPartyRepository partyRepository,
    IEmailSender emailSender) : IRequestHandler<SendInvitationCommand>
{
    private const string AppBaseUrl = "https://seuapp.com";

    public async Task Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var party = await partyRepository.GetByIdAsync(request.PartyId, request.OwnerUserId, cancellationToken);
        if (party is null) return;

        var guest = party.Convites.SelectMany(c => c.Guests).FirstOrDefault(g => g.Id == request.GuestId);
        if (guest is null || string.IsNullOrWhiteSpace(guest.Email)) return;

        var link = $"{AppBaseUrl}/convite/{guest.InvitationToken}";
        var html = BuildInvitationHtml(party.Name, party.Date, party.Time, party.Location, guest.Name, link);

        await emailSender.SendAsync(
            guest.Email,
            $"Você foi convidado para {party.Name}!",
            html,
            cancellationToken);
    }

    private static string BuildInvitationHtml(
        string partyName,
        string date,
        string time,
        string location,
        string guestName,
        string invitationLink) => $"""
        <!DOCTYPE html>
        <html lang="pt-BR">
        <head><meta charset="UTF-8" /></head>
        <body style="font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 32px;">
          <div style="max-width: 520px; margin: 0 auto; background: #ffffff; border-radius: 12px; padding: 32px; box-shadow: 0 2px 8px rgba(0,0,0,0.08);">
            <h1 style="color: #7c3aed; margin-bottom: 4px;">🎉 Você foi convidado!</h1>
            <p style="color: #374151; font-size: 16px;">Olá, <strong>{guestName}</strong>!</p>
            <p style="color: #374151; font-size: 16px;">
              Você recebeu um convite para o evento <strong>{partyName}</strong>.
            </p>
            <table style="width: 100%; margin: 24px 0; border-collapse: collapse;">
              <tr>
                <td style="padding: 8px 0; color: #6b7280; font-size: 14px;">📅 Data</td>
                <td style="padding: 8px 0; color: #111827; font-size: 14px;"><strong>{date}</strong></td>
              </tr>
              <tr>
                <td style="padding: 8px 0; color: #6b7280; font-size: 14px;">🕐 Horário</td>
                <td style="padding: 8px 0; color: #111827; font-size: 14px;"><strong>{time}</strong></td>
              </tr>
              <tr>
                <td style="padding: 8px 0; color: #6b7280; font-size: 14px;">📍 Local</td>
                <td style="padding: 8px 0; color: #111827; font-size: 14px;"><strong>{location}</strong></td>
              </tr>
            </table>
            <a href="{invitationLink}"
               style="display: inline-block; background-color: #7c3aed; color: #ffffff; text-decoration: none;
                      padding: 14px 28px; border-radius: 8px; font-size: 16px; font-weight: bold; margin-top: 8px;">
              Confirmar presença
            </a>
            <p style="color: #9ca3af; font-size: 12px; margin-top: 24px;">
              Se não conseguir clicar no botão, acesse: {invitationLink}
            </p>
          </div>
        </body>
        </html>
        """;
}
