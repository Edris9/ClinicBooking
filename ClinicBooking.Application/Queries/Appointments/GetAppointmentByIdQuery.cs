using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;



public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
{
    public int Id { get; set; }
}