using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ClinicDbContext context) : base(context) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}