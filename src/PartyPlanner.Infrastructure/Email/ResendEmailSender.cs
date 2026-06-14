using PartyPlanner.Core.Interfaces;
using Resend;

namespace PartyPlanner.Infrastructure.Email;

public sealed class ResendEmailSender(IResend resend) : IEmailSender
{
    private const string FromAddress = "onboarding@resend.dev";

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var message = new EmailMessage
        {
            From = FromAddress,
            Subject = subject,
            HtmlBody = htmlBody,
        };
        message.To.Add(to);

        await resend.EmailSendAsync(message, cancellationToken);
    }
}
