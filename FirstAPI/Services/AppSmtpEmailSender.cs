using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace FirstAPI.Services;

public sealed class AppSmtpEmailSender : IAppEmailSender
{
    private readonly SmtpOptions _o;
    public AppSmtpEmailSender(IOptions<SmtpOptions> opts) => _o = opts.Value;

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        to = to?.Trim();
        if (string.IsNullOrWhiteSpace(to) || !MailAddress.TryCreate(to, out var toAddr))
            throw new ArgumentException("Invalid recipient email.", nameof(to));

        if (string.IsNullOrWhiteSpace(_o.Host) || _o.Port <= 0 ||
            string.IsNullOrWhiteSpace(_o.User) || string.IsNullOrWhiteSpace(_o.Pass) ||
            string.IsNullOrWhiteSpace(_o.From))
            throw new InvalidOperationException("SMTP config missing (Host/Port/User/Pass/From).");

        var fromAddr = new MailAddress(_o.From, "Task App");

        using var msg = new MailMessage(fromAddr, toAddr)
        {
            Subject = subject ?? string.Empty,
            Body = htmlBody ?? string.Empty,
            IsBodyHtml = true
        };

        using var client = new SmtpClient(_o.Host, _o.Port)
        {
            EnableSsl = _o.EnableSsl,
            Credentials = new NetworkCredential(_o.User, _o.Pass),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 15000
        };

        await client.SendMailAsync(msg);
    }
}
