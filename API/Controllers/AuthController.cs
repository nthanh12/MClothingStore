using Application.UserModules.Abstracts;
using Application.UserModules.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ApiResponse Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                return new(_authService.Login(loginDto));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("register")]
        public ApiResponse Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                _authService.RegisterUser(createUserDto);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
