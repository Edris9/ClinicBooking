using System;
using System.Collections.Generic;


public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);

}