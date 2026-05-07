


public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, int>
{
    private readonly IDoctorRepository _doctorRepository;

    public CreateDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    public async Task<int> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = new Doctor
        {
            Name = request.Name,
            Specialty = request.Specialty,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DepartmentId = request.DepartmentId
        };
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return doctor.Id;
    }
}