// File: ClinicBooking.Notifications/Models/EmailRequest.cs
using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Notifications.Models
{
    /// <summary>
    /// DTO that represents the payload sent to the Notifications service
    /// for sending an e‑mail.
    /// </summary>
    public class EmailRequest
    {
        [Required]
        public string To { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;
    }
}
