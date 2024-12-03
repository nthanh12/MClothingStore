using Application.UserModules.Abstracts;
using Application.UserModules.DTOs;
using Domain.Entities;
using Infrastructure.Persistances;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts.Exceptions;
using Shared.Exceptions;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public TokenApiDto Login(UserLoginDto loginDto)
        {
            _logger.LogInformation($"{nameof(Login)}: input = {JsonSerializer.Serialize(loginDto)}");
            var user = _context.Users.FirstOrDefault(x => x.Username == loginDto.Username);
            if (user == null)
            {
                throw new UserFriendlyException(ErrorCode.UserNotFound);
            }
            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.Password))
            {
                throw new UserFriendlyException(ErrorCode.PasswordWrong);
            }
            var newAccessToken = CreateToken(loginDto);
            _context.SaveChangesAsync();

            return new TokenApiDto
            {
                AccessToken = newAccessToken
            };
        }

        public void RegisterUser(CreateUserDto createUserDto)
        {
            _logger.LogInformation($"{nameof(RegisterUser)}: input = {JsonSerializer.Serialize(createUserDto)}");
            var check = _context.Users.FirstOrDefault(x => x.Username == createUserDto.Username);

            if(check !=  null)
            {
                throw new UserFriendlyException(ErrorCode.UsernameIsExist);
            }

            if(createUserDto.Password.Length < 6)
            {
                throw new UserFriendlyException(ErrorCode.PasswordMustBeLongerThanSixCharacter);
            }

            if (!(Regex.IsMatch(createUserDto.Password, "[a-z]") && Regex.IsMatch(createUserDto.Password, "[A-Z]") && Regex.IsMatch(createUserDto.Password, "[0-9]")))
            {
                throw new UserFriendlyException(ErrorCode.TypeofPasswordMustBeNumberOrString);
            }

            if (!Regex.IsMatch(createUserDto.Password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                throw new UserFriendlyException(ErrorCode.PasswordMustBeContainsSpecifyCharacter);

            _context.Users.Add(new User
            {
                Username = createUserDto.Username,
                Password = PasswordHasher.HassPassword(createUserDto.Password),
                UserType = createUserDto.UserType
            });

            _context.SaveChanges();
        }
        private string CreateToken(UserLoginDto user)
        {
            var jwtToken = new JwtSecurityTokenHandler();
            var userId = _context.Users.FirstOrDefault(u => u.Username == user.Username) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT")["Key"]);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{userId.Id}"),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("user_type", userId.UserType.ToString()),
                new Claim("user_id", userId.Id.ToString())
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: credentials
            );
            return jwtToken.WriteToken(token);
        }
    }
}
