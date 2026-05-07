
public class CancelAppointmentCommand : IRequest<int>
{
    public int AppointmentId { get; set; }
}