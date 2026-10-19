namespace LoanApplicationService.Api.Services;

// Ge mig ett personnummer, så ska jag kunna få tillbaka ett kreditvärde.
public interface ICreditScoreService
{
    Task<int> GetCreditScoreAsync(
        string socialSecurityNumber);
}
