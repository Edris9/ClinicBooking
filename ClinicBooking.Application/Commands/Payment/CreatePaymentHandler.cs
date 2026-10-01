using ClinicBooking.Domain.Entities;
using ClinicBooking.Domain.Enums;
using MediatR;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, int>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public CreatePaymentHandler(IPaymentRepository paymentRepository, IAppointmentRepository appointmentRepository)
    {
        _paymentRepository = paymentRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
            throw new Exception("Appointment not found");

        var now = DateTime.UtcNow;

        var payment = new Payment
        {
            AppointmentId = request.AppointmentId,
            Amount = request.Amount,
            Reason = request.Reason,
            CreatedAt = now,
            DueDate = now.AddDays(30),
            Status = PaymentStatus.Unpaid
        };

        await _paymentRepository.AddAsync(payment);
        return payment.Id;
    }
}