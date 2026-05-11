

public class DeletePatientHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IPatientRepository _patientRepository;
    public DeletePatientHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    public async Task<Unit> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(command.Id);
        if (patient == null)
        {
            throw new NotFoundException("Patient not found");
        }
        await _patientRepository.DeleteAsync(command.Id);
        return Unit.Value;
    }
}