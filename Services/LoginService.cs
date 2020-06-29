using Core.Exceptions;
using Core.Filters.Login;
using Core.Models.Login.DTOs;
using Core.Models.Users;
using Core.Models.Users.DTOs;
using Core.Services;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;
using Service.Validators;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LoginService : ILoginService
    {
        public async Task<LoginOutputDTO> Login(UserLoginFilter filter, string secret)
        {
            var result = await ValidateFields(filter).ConfigureAwait(false);

            if (!result.IsValid)
                throw new BusinessException(result.Errors.Select(x => x.ErrorMessage).LastOrDefault());

            User user = null;

            if (filter.Email.ToLower().Equals("valid-user@gmail.com")) {                
                // fake valid user from database
                user = new User(){
                    Email = "valid-user@gmail.com",
                    Name = "Valid User",
                    Id = 1};
            }; 

            if (user == null) 
                 throw new BusinessException("User not found or not authorized!");

            var token = GenerateToken(user, secret);
            
           
            LoginOutputDTO outputDTO = BuildOutputDTO(user, token);

            return outputDTO;
        }

        private static LoginOutputDTO BuildOutputDTO(User user, string token)
        {
            return new LoginOutputDTO
            {
                User = new UserOutputDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                },
                AccessToken = token
            };
        }

        private static string GenerateToken(User user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "AuthorizedUser")
                }),
                Expires = DateTime.UtcNow.AddDays(0.25),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            
            return tokenHandler.WriteToken(token);
        }

        private async Task<ValidationResult> ValidateFields(UserLoginFilter userLogin)
        {
            var validator = new LoginValidatorService();
            var result = await validator.ValidateAsync(userLogin).ConfigureAwait(true);
            return result;
        }
    }
}