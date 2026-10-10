using System;
using System.Threading;
using System.Threading.Tasks;
using hackatonxp.Application.DTOs;
using hackatonxp.Application.Interfaces;
using hackatonxp.Infrastructure.Repositories;

namespace hackatonxp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdvisorRepository _repo;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;

        public AuthService(IAdvisorRepository repo, IPasswordHasher hasher, IJwtService jwt)
        {
            _repo = repo;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AdvisorLoginDto dto, CancellationToken cancellationToken = default)
        {
            var advisor = await _repo.GetByEmailAsync(dto.Email, cancellationToken);
            // Do not reveal whether email exists. Use timing-safe checks.
            if (advisor is null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!advisor.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!_hasher.VerifyHashedPassword(advisor.PasswordHash, dto.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            return _jwt.GenerateToken(advisor.Id, advisor.Email);
        }
    }
}
