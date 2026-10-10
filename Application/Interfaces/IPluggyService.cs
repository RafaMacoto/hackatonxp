using hackatonxp.Application.DTOs;
using System.Text.Json;

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
    }

  


    }