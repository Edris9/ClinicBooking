using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
{
    public int Id { get; set; }
}