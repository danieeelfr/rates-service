using Core.Exceptions;
using Core.Filters.Login;
using Core.Models.Login.DTOs;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace RatesAPI.Controllers
{
    [Route("api/v0/auth")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IConfiguration _configuration;

        private IMemoryCache _cache;

        public SecurityController(ILoginService loginService, IConfiguration configuration, IMemoryCache memoryCache)
        {
            this._loginService = loginService;
            this._configuration = configuration;
            this._cache = memoryCache;
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

                LoginOutputDTO output;

                if (!_cache.TryGetValue(CacheKeys.Entry, out output))
                {
                    output = await _loginService.Login(filter, key).ConfigureAwait(true);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                    if (output.AccessToken == null)
                        return Forbid();

                    _cache.Set(CacheKeys.Entry, output, cacheEntryOptions);

                }

                return Ok(output);
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

    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }
}