namespace ClinicBooking.Client.Models
{
    public class CreatePaymentDto
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        
    }
}
