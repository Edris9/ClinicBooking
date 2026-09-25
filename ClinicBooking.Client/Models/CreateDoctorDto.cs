namespace ClinicBooking.Client.Models
{
    public class CreateDoctorDto
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; } = 0;
    }
}
