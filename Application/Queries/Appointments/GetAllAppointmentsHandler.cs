using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public GetAllAppointmentsHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }
}