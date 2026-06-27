using MediatR;
using PartyPlanner.Application.Common;
using PartyPlanner.Core.Entities;
using PartyPlanner.Core.Interfaces;
using PartyPlanner.Core.Interfaces.Repositories;

namespace PartyPlanner.Application.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IAuthRepository authRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork,
    AppOptions appOptions) : IRequestHandler<ForgotPasswordCommand>
{
    private static readonly TimeSpan TokenExpiry = TimeSpan.FromHours(1);

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await authRepository.GetUserByEmailAsync(email, cancellationToken);

        // Sempre retorna sem erro para não revelar se o email existe
        if (user is null) return;

        var rawToken = Convert.ToHexString(Guid.NewGuid().ToByteArray()) +
                       Convert.ToHexString(Guid.NewGuid().ToByteArray());
        var token = new EntityPasswordResetToken(
            Guid.NewGuid(),
            user.Id,
            rawToken,
            DateTime.UtcNow.Add(TokenExpiry));

        await authRepository.AddPasswordResetTokenAsync(token, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var link = $"{appOptions.BaseUrl}/reset-senha/{rawToken}";
        var html = BuildResetEmail(user.Name, link);
        await emailSender.SendAsync(email, "Redefinição de senha — Celebra", html, cancellationToken);
    }

    private static string BuildResetEmail(string name, string link) => $"""
        <!DOCTYPE html>
        <html lang="pt-BR">
        <head><meta charset="UTF-8" /></head>
        <body style="font-family:Arial,sans-serif;background:#f9f9f9;padding:32px;">
          <div style="max-width:520px;margin:0 auto;background:#fff;border-radius:12px;padding:32px;box-shadow:0 2px 8px rgba(0,0,0,.08);">
            <h1 style="color:#7c3aed;margin-bottom:4px;">🔐 Redefinição de senha</h1>
            <p style="color:#374151;font-size:16px;">Olá, <strong>{name}</strong>!</p>
            <p style="color:#374151;font-size:16px;">
              Recebemos uma solicitação para redefinir a senha da sua conta no <strong>Celebra</strong>.
            </p>
            <p style="color:#6b7280;font-size:14px;">Este link é válido por <strong>1 hora</strong>.</p>
            <a href="{link}"
               style="display:inline-block;background:#7c3aed;color:#fff;text-decoration:none;
                      padding:14px 28px;border-radius:8px;font-size:16px;font-weight:bold;margin-top:8px;">
              Redefinir senha
            </a>
            <p style="color:#9ca3af;font-size:12px;margin-top:24px;">
              Se você não solicitou isso, ignore este email.<br/>
              Link: {link}
            </p>
          </div>
        </body>
        </html>
        """;
}
