

public class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;
    public GetDoctorByIdHandler(IDoctorRepository doctorRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }
    public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.Id);
        return _mapper.Map<DoctorDto>(doctor);
    }
}