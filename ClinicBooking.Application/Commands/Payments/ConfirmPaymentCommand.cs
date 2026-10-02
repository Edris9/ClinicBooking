using MediatR;

/// <summary>
/// Command used to confirm a payment from the link the patient received.
/// </summary>
public sealed class ConfirmPaymentCommand : IRequest
{
    public string Token { get; }

    public ConfirmPaymentCommand(string token)
    {
        Token = token ?? throw new ArgumentNullException(nameof(token));
    }
}
