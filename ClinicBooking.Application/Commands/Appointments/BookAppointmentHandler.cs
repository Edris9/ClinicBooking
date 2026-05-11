using MediatR;
using AutoMapper;
public class BookAppointmentHandler : IRequestHandler<BookAppointmentCommand, int>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    public BookAppointmentHandler(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
    }
    public async Task<int> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.PatientId);
        if (patient == null)
            throw new Exception("Patient not found");
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
            throw new Exception("Doctor not found");
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ScheduledAt = request.ScheduledAt,
            Reason = request.Reason,
            Status = AppointmentStatus.Scheduled
        };
        await _appointmentRepository.AddAsync(appointment);
        return appointment.Id;
    }
}