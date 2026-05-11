using MediatR;
using AutoMapper;

public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, int>
{
    private readonly IAppointmentRepository _appointmentRepository;
    public CancelAppointmentHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<int> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
            throw new Exception("Appointment not found");
        appointment.Status = AppointmentStatus.Cancelled;
        await _appointmentRepository.UpdateAsync(appointment);
        return appointment.Id;
    }
}