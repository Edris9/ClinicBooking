using MediatR;

public class DeletePaymentCommand : IRequest
{
    public int Id { get; set; }
}