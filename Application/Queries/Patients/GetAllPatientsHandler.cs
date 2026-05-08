using System;


public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    public GetAllPatientsHandler(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }
    public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync();
        return _mapper.Map<List<PatientDto>>(patients);
    }
}