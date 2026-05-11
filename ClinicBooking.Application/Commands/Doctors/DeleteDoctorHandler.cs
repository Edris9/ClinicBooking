using MediatR;
using AutoMapper;


public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IDoctorRepository _doctorRepository;
    public DeleteDoctorHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.Id);
        if (doctor == null)
        {
            throw new Exception("Doctor not found");
        }
        await _doctorRepository.DeleteAsync(request.Id);
    }
}