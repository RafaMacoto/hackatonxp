using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hackatonxp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdvisorLoginDto dto)
        {
            try
            {
                var res = await _auth.AuthenticateAsync(dto);
                return Ok(res);
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
