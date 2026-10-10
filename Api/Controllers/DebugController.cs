using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace hackatonxp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly IAdvisorRepository _repo;
        private readonly IPasswordHasher _hasher;

        public DebugController(IAdvisorRepository repo, IPasswordHasher hasher)
        {
            _repo = repo;
            _hasher = hasher;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] AdvisorLoginDto dto)
        {
            var adv = await _repo.GetByEmailAsync(dto.Email);
            if (adv is null) return NotFound(new { message = "advisor_not_found" });

            var ok = _hasher.VerifyHashedPassword(adv.PasswordHash, dto.Password);
            return Ok(new { email = adv.Email, passwordHash = adv.PasswordHash, verify = ok });
        }
    }
}
