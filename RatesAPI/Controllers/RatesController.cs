﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace RatesAPI.Controllers
{
    [Route("api/v0")]
    [ApiController]
    [Authorize (Roles = "AuthorizedUser")]
    public class RatesController : ControllerBase
    {
        [HttpGet("taxajuros")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetInterestRate()
        {
            try
            {
                var result = 1 * 0.01;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}