using MediatR;

public class RegisterCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}