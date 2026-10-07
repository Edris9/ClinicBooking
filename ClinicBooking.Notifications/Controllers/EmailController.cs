// File: ClinicBooking.Notifications/Controllers/EmailController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Notifications.Models;
using ClinicBooking.Notifications.Services;

namespace ClinicBooking.Notifications.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // POST /api/email
    public class EmailController : ControllerBase
    {
        private readonly BrevoEmailService _emailService;

        public EmailController(BrevoEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Skickar ett e‑mail via Brevo‑leverantören.
        /// </summary>
        /// <param name="request">Mottagare, ämne och brödtext.</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmailRequest request)
        {

            // Brevo‑klienten har en async‑metod som skickar mejlet.
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);

            // Enkelt 200‑OK – inget innehåll behövs.
            return Ok();
        }
    }
}
