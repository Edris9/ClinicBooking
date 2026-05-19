using MediatR;

using System.Threading;
using System.Threading.Tasks;
using System;
public class RegisterHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role
        };

        await _userRepository.AddAsync(user);
        return _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email, user.Role);
    }
}