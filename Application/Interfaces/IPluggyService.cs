
using hackatonxp.Application.DTOs;

namespace hackatonxp.Application.Interfaces
{
    public interface IPluggyService
    {
        Task<List<InvestmentResponseDto>> GetClientInvestmentsAsync(
            string itemId,
            CancellationToken cancellationToken = default);

        Task<List<LoanResponse>> GetClientLoansAsync(
            string itemId,
            CancellationToken cancellationToken = default);

        Task<AccountResponseDto?> GetClientAccountAsync(
            string itemId,
            CancellationToken cancellationToken = default);
    }
}
