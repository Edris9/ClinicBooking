using System;


public class AppointmentDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Reason { get; set; }
}