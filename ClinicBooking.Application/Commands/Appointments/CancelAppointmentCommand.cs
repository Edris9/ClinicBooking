using MediatR;
using AutoMapper;
public class CancelAppointmentCommand : IRequest<int>
{
    public int AppointmentId { get; set; }
}