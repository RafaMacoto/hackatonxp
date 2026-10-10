using System;

namespace hackatonxp.Application.DTOs
{
    public class AdvisorCreateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AdvisorLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AdvisorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAt { get; set; }
    }
}
