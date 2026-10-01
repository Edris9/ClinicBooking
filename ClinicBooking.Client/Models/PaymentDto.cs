namespace ClinicBooking.Client.Models
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? VerificationSentAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}