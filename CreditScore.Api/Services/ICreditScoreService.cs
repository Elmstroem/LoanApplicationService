namespace CreditScore.Api.Services;

public interface ICreditScoreService
{
    Task<int?> GetCreditScoreAsync(
        string socialSecurityNumber);

    Task<int> CreateCreditScoreAsync(
        string socialSecurityNumber,
        string name,
        decimal annualIncome,
        decimal totalDebt);

    Task<int?> UpdateCreditScoreAsync(
        string socialSecurityNumber,
        decimal annualIncome,
        decimal totalDebt);
}