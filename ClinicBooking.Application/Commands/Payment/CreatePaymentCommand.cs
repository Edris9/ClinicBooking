using MediatR;

public class CreatePaymentCommand : IRequest<int>
{
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; }
}