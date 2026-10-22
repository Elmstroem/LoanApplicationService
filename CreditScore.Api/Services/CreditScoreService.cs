using CreditScore.Api.Models;
using CreditScore.Api.Repositories;

namespace CreditScore.Api.Services;

public class CreditScoreService : ICreditScoreService
{
    private readonly ICustomerRepository _customerRepository;

    public CreditScoreService(
        ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<int?> GetCreditScoreAsync(
        string socialSecurityNumber)
    {
        Customer? customer =
            await _customerRepository
                .GetBySocialSecurityNumberAsync(
                    socialSecurityNumber);

        if (customer is null)
        {
            return null;
        }

        if (customer.AnnualIncome <= 0)
        {
            return 0;
        }

        decimal score =
            100 -
            (
                customer.TotalDebt /
                customer.AnnualIncome *
                100
            );

        score = Math.Clamp(
            score,
            0,
            100);

        int roundedScore =
            (int)Math.Round(
                score,
                MidpointRounding.AwayFromZero);

        return roundedScore;
    }
}