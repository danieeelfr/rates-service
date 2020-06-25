using Core.Exceptions;
using Core.Filters.Login;
using Core.Models.Login.DTOs;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace RatesAPI.Security.Controllers
{
    [Route("api/v0/auth")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IConfiguration _configuration;

        public SecurityController(ILoginService loginService, IConfiguration configuration)
        {
            this._loginService = loginService;
            this._configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginOutputDTO>> Login([FromBody] UserLoginFilter filter)
        {
            try
            {
                var key = _configuration.GetValue<string>("JwtToken:SecretKey");
          
                var userWithToken = await _loginService.Login(filter, key).ConfigureAwait(true);

                if (userWithToken == null) 
                { 
                    return Forbid(); 
                };

                return Ok(userWithToken);
            }
            catch (BusinessException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}