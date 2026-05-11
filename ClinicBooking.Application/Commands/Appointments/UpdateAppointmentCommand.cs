using MediatR;
using AutoMapper;


public class UpdateAppointmentCommand: IRequest<int>
{
    public int Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
}
