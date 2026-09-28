namespace ClinicBooking.Client.Models
{
    public class CreatePatientDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? ResponsiblePhysician { get; set; }
    }
}
