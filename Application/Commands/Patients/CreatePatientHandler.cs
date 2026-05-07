

using System;


public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, int>
{
    private readonly IPatientRepository _patientRepository;
    public CreatePatientHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    public async Task<int> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            Name = command.Name,
            DateOfBirth = command.DateOfBirth,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        await _patientRepository.AddAsync(patient);
        return patient.Id;
    }
}
