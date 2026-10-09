
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public DepartmentsController(ClinicDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _context.Departments
            .Select(d => new
            {
                d.Id,
                d.Name
            })
            .ToListAsync();

        return Ok(departments);
    }
}