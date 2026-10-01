using System;
using MediatR;
using AutoMapper;

public class GetAllPaymentsHandler : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    public GetAllPaymentsHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }
    public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetAllAsync();
        return _mapper.Map<List<PaymentDto>>(payments);
    }
}