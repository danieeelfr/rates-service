﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace RatesAPI.Controllers
{
    [Route("api/v0")]
    [ApiController]
   
    public class RatesController : ControllerBase
    {
        [Authorize (Roles = "AuthorizedUser")]
        [HttpGet("taxajurosjwt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetRateUsingJwt()
        {
           return GetInterestRate();
        }

        [HttpGet("taxajuros")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetRate()
        {
            return GetInterestRate();
        }

        private ActionResult<double> GetInterestRate()
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