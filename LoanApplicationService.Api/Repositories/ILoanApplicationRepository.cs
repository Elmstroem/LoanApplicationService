using LoanApplicationService.Api.Models;

namespace LoanApplicationService.Api.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplication> CreateAsync(
        LoanApplication loanApplication);
}