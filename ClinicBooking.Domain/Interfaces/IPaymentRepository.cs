using System.Threading.Tasks;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment?> GetByVerificationHashAsync(string hash);
}
