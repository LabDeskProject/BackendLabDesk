namespace Application.Interface.Security;

public interface IRefreshTokenService
{
    string GenerateToken();
    string HashToken(string token);
}
