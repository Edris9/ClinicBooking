using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int departmentId);
}