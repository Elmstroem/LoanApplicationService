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


    // ============================================
    // GET - Hämta och beräkna Credit Score
    // ============================================

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

        return CalculateCreditScore(customer);
    }


    // ============================================
    // CREATE - Skapa kund och Credit Score
    // ============================================

    public async Task<int> CreateCreditScoreAsync(
        string socialSecurityNumber,
        string name,
        decimal annualIncome,
        decimal totalDebt)
    {
        Customer? existingCustomer =
            await _customerRepository
                .GetBySocialSecurityNumberAsync(
                    socialSecurityNumber);

        if (existingCustomer is not null)
        {
            throw new ArgumentException(
                "En kund med detta personnummer finns redan.");
        }

        var customer = new Customer
        {
            SocialSecurityNumber = socialSecurityNumber,
            Name = name,
            AnnualIncome = annualIncome,
            TotalDebt = totalDebt
        };

        Customer createdCustomer =
            await _customerRepository
                .CreateAsync(customer);

        return CalculateCreditScore(createdCustomer);
    }


    // ============================================
    // UPDATE - Uppdatera kund och Credit Score
    // ============================================

    public async Task<int?> UpdateCreditScoreAsync(
        string socialSecurityNumber,
        decimal annualIncome,
        decimal totalDebt)
    {
        Customer? updatedCustomer =
            await _customerRepository
                .UpdateAsync(
                    socialSecurityNumber,
                    annualIncome,
                    totalDebt);

        if (updatedCustomer is null)
        {
            return null;
        }

        return CalculateCreditScore(updatedCustomer);
    }


    // ============================================
    // CREDIT SCORE BERÄKNING
    // ============================================

    private static int CalculateCreditScore(
        Customer customer)
    {
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

        return (int)Math.Round(
            score,
            MidpointRounding.AwayFromZero);
    }
}