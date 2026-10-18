using Domain.User;

namespace Application.Interface.Security;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}
