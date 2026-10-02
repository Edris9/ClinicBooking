using Microsoft.EntityFrameworkCore;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ClinicDbContext context) : base(context) { }

    public async Task<Payment?> GetByVerificationHashAsync(string hash)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.VerificationCodeHash == hash);
    }
}