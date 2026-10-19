using LoanApplicationService.Api.Models;

namespace LoanApplicationService.Api.Services;

// En class som implementerar ILoanApplicationService måste kunna skapa en låneansökan på det här sättet.
public interface ILoanApplicationService
{
    Task<LoanApplication> CreateLoanApplicationAsync(
        string socialSecurityNumber,
        int amount);
}