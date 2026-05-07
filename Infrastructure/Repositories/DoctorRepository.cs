using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId)
    {
        return await _context.Doctors
            .Where(d => d.DepartmentId == departmentId)
            .ToListAsync();
    }
}