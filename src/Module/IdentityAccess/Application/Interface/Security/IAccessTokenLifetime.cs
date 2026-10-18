namespace Application.Interface.Security;

public interface IAccessTokenLifetime
{
    int Minutes { get; }
}
