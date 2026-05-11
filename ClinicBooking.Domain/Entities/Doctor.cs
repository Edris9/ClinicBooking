using System;
using System.Collections.Generic;


public class Doctor
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Specialty { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public int DepartmentId { get; set; }
	public bool IsAvailable { get; set; }
	public Department Department { get; set; }
	public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}