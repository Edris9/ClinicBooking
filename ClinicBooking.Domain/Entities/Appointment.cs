using System;
using System.Collections.Generic;
using System.Linq;


public class Appointment
{
    public int Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Time { get; set; }
    public string Reason { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public AppointmentStatus Status { get; set; }

}