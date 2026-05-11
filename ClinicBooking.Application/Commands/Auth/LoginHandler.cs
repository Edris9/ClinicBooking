using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
public class LoginHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || user.Password != request.Password)
            throw new Exception("Invalid credentials");

        return _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email, user.Role);
    }
}