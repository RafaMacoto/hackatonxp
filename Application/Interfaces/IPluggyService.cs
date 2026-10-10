using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IPluggyService
    {
        Task<List<InvestmentResponse>> GetClientInvestmentsAsync(
            string itemId,
            CancellationToken cancellationToken = default);
    }
}