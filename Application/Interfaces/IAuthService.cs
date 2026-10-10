using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(AdvisorLoginDto dto, CancellationToken cancellationToken = default);
    }
}
