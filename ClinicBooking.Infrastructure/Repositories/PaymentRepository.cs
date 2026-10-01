using ClinicBooking.Domain.Entities;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ClinicDbContext context) : base(context) { }
}