using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;


namespace ClinicBooking.Notifications.Services
{

    public class BrevoEmailService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public BrevoEmailService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", _config["Brevo:ApiKey"]);
            request.Content = JsonContent.Create(new
            {
                sender = new { name = "Kliniken", email = _config["Brevo:SenderEmail"] },
                to = new[] { new { email = to } },
                subject,
                htmlContent = body
            });

            var response = await _http.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}