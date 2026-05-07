

using System;
using System.Collections.Generic;


public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
}

