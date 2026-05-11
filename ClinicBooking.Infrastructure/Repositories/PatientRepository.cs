using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ClinicDbContext context) : base(context)
    {

    }
    public async Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId)
    {
        return await _context.Appointments
            .Where(a => a.Doctor.DepartmentId == departmentId)
            .Select(a => a.Patient)
            .Distinct()
            .ToListAsync();
    }
    public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Select(a => a.Patient)
            .Distinct()
            .ToListAsync();
    }
}