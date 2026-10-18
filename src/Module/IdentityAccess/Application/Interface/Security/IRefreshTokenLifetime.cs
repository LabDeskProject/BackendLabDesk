namespace Application.Interface.Security;

public interface IRefreshTokenLifetime
{
    int Days { get; }
    int AbsoluteDays { get; }
}
