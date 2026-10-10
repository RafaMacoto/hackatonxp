
using hackatonxp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hackatonxp.Api.Controllers
{
    [ApiController]
    [Route("api/pluggy")]
    [Authorize]
    public class PluggyController : ControllerBase
    {
        private readonly IPluggyService _pluggyService;

        public PluggyController(IPluggyService pluggyService)
        {
            _pluggyService = pluggyService;
        }

        [HttpGet("investments")]
        public async Task<IActionResult> GetInvestments(
            [FromQuery] string itemId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return BadRequest(new
                {
                    message = "O itemId é obrigatório."
                });
            }

            try
            {
                var investments =
                    await _pluggyService.GetClientInvestmentsAsync(
                        itemId,
                        cancellationToken);

                return Ok(investments);
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new
                    {
                        message = "Não foi possível consultar os investimentos na Pluggy."
                    });
            }
        }

        [HttpGet("loans")]
        public async Task<IActionResult> GetLoans(
            [FromQuery] string itemId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return BadRequest(new
                {
                    message = "O itemId é obrigatório."
                });
            }

            try
            {
                var loans =
                    await _pluggyService.GetClientLoansAsync(
                        itemId,
                        cancellationToken);

                return Ok(loans);
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new
                    {
                        message = "Não foi possível consultar os empréstimos na Pluggy."
                    });
            }
        }
    }
}
