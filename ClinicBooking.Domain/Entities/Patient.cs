using System;
using System.Collections.Generic;


public class Patient
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}