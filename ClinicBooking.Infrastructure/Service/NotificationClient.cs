using System.Net.Http.Json;

public class NotificationClient : IEmailService
{
    private readonly HttpClient _http;

    public NotificationClient(HttpClient http)
    {
        _http = http;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("api/email", new { to, subject, body }, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}