using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId);
}