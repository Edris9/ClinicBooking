using MediatR;

public class UpdatePaymentCommand : IRequest<int>
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; }
  

}