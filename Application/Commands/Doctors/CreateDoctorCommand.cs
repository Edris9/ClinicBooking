

using System.Net;

public class CreateDoctorCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Specialty { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
}