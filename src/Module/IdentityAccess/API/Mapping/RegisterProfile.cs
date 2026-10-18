using IdentityAccess.API.Contracts.Register;
using Application.Register;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IdentityAccess.API.Mapping
{
    public sealed class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterUserRequestDTO, RegisterUserCommand>();
            CreateMap<RegisterUserResult, RegisterUserResponseDTO>();
        }
    }
}
