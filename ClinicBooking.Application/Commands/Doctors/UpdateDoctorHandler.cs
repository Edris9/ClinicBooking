using MediatR;
using AutoMapper;

public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand>
{
    private readonly IDoctorRepository _doctorRepository;
    public UpdateDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    public async Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.Id);
        if (doctor == null)
        {
            throw new Exception("Doctor not found");
        }
        doctor.Name = request.Name;
        doctor.Specialty = request.Specialty;
        doctor.Email = request.Email;
        doctor.PhoneNumber = request.PhoneNumber;
        doctor.DepartmentId = request.DepartmentId;
        await _doctorRepository.UpdateAsync(doctor);
        
    }
}