using MediatR;


public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    public DeletePaymentHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public async Task Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.Id);
        if (payment == null)
            throw new Exception("Payment not found");

        await _paymentRepository.DeleteAsync(request.Id);
    }
}
