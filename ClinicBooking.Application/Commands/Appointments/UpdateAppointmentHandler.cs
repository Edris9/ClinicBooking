using MediatR;
using AutoMapper;


public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, int>
{
    private readonly IAppointmentRepository _appointmentRepository;
    public UpdateAppointmentHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.Id);
        if (appointment == null)
            throw new Exception("Appointment not found");
        appointment.ScheduledAt = request.ScheduledAt;
        appointment.Reason = request.Reason;
        appointment.PatientId = request.PatientId;
        appointment.DoctorId = request.DoctorId;
        await _appointmentRepository.UpdateAsync(appointment);
        return appointment.Id;
    }
}