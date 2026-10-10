using System;
using System.Threading;
using System.Threading.Tasks;
using Advisor360.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advisor360.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientOverviewService _overviewService;

        public ClientsController(IClientOverviewService overviewService)
        {
            _overviewService = overviewService;
        }

        /// <summary>
        /// Obtém a visão consolidada de um cliente por id.
        /// </summary>
        /// <param name="id">Identificador do cliente (GUID).</param>
        [HttpGet("{id:guid}/overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOverview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var overview = await _overviewService.GetOverviewAsync(id, cancellationToken);
            if (overview is null)
                return NotFound();

            return Ok(overview);
        }
    }
}
