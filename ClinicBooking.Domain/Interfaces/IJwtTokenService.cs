
public interface IJwtTokenService
{
    string GenerateToken(string userId, string email, string role);
}