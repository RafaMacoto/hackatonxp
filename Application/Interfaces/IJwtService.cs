using System;
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IJwtService
    {
        AuthResponseDto GenerateToken(Guid advisorId, string email);
    }
}
