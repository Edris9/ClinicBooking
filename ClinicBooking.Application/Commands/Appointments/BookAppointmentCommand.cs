using MediatR;
using AutoMapper;

public class BookAppointmentCommand : IRequest<int>
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
}
