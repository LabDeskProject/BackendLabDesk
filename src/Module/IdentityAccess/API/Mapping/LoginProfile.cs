using Application.Login;
using AutoMapper;
using IdentityAccess.API.Contracts.Login;

namespace IdentityAccess.API.Mapping
{
    public sealed class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginUserRequestDto, LoginUserCommand>();

            CreateMap<LoginUserResult, LoginUserResponseDto>();
        }
    }
}
