using Application.UserModules.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IAuthService
    {
        public TokenApiDto Login(UserLoginDto loginDto);

        public void RegisterUser(CreateUserDto createUserDto);

    }
}
