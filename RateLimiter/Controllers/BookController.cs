﻿using Microsoft.AspNetCore.Mvc;

namespace RateLimiter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // [EnableRateLimiting("fixed")]
        // [EnableRateLimiting("sliding")]
        // [EnableRateLimiting("token")]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
