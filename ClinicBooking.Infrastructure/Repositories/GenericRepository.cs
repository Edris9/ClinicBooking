using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ClinicDbContext _context;

    public GenericRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    public async Task<T> GetByIdAsync(int id) =>
        await _context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity) =>
        await _context.Set<T>().AddAsync(entity);

    public async Task UpdateAsync(T entity) =>
        _context.Set<T>().Update(entity);

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        _context.Set<T>().Remove(entity);
    }
}