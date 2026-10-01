using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;



public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, int>
{
    private readonly IPaymentRepository _paymentRepository;
    public UpdatePaymentHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public async Task<int> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.Id);
        if (payment == null)
        {
            throw new Exception($"Payment with ID {request.Id} not found.");
        }
        payment.Amount = request.Amount;
        payment.Reason = request.Reason;
       
        await _paymentRepository.UpdateAsync(payment);
        return payment.Id;
    }
}