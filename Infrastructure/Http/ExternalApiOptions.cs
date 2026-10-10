namespace Advisor360.Infrastructure.Http
{
    public class ExternalApiOptions
    {
        public string? BaseAddress { get; set; }
        public int TimeoutSeconds { get; set; } = 30;
    }
}
