using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IPatientRepository _patientRepository;

    public UpdatePatientHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id);
        if (patient == null)
            throw new Exception("Patient not found");

        patient.Name = request.Name;
        patient.Email = request.Email;
        patient.PhoneNumber = request.PhoneNumber;
        patient.DateOfBirth = request.DateOfBirth;

        await _patientRepository.UpdateAsync(patient);
        
    }
}