using CreditScore.Api.Models;

namespace CreditScore.Api.Repositories;

public class InMemoryCustomerRepository
    : ICustomerRepository
{
    private readonly List<Customer> _customers =
    [
        new Customer
        {
            Id = 1,
            SocialSecurityNumber = "19900101-2010",
            Name = "Anna Svensson",
            AnnualIncome = 520000m,
            TotalDebt = 150000m
        },

        new Customer
        {
            Id = 2,
            SocialSecurityNumber = "19850515-1234",
            Name = "Erik Johansson",
            AnnualIncome = 360000m,
            TotalDebt = 180000m
        },

        new Customer
        {
            Id = 3,
            SocialSecurityNumber = "19780320-5678",
            Name = "Maria Lindberg",
            AnnualIncome = 680000m,
            TotalDebt = 1200000m
        }
    ];

    private int _nextId = 4;

    private readonly object _lock = new();


    // ============================================
    // GET - Hämta kund via personnummer
    // ============================================

    public Task<Customer?> GetBySocialSecurityNumberAsync(
        string socialSecurityNumber)
    {
        Customer? customer =
            _customers.FirstOrDefault(
                customer =>
                    customer.SocialSecurityNumber ==
                    socialSecurityNumber);

        return Task.FromResult(customer);
    }


    // ============================================
    // CREATE - Skapa ny kund
    // ============================================

    public Task<Customer> CreateAsync(
        Customer customer)
    {
        lock (_lock)
        {
            customer.Id = _nextId;

            _nextId++;

            _customers.Add(customer);
        }

        return Task.FromResult(customer);
    }


    // ============================================
    // UPDATE - Uppdatera kund
    // ============================================

    public Task<Customer?> UpdateAsync(
        string socialSecurityNumber,
        decimal annualIncome,
        decimal totalDebt)
    {
        lock (_lock)
        {
            Customer? customer =
                _customers.FirstOrDefault(
                    customer =>
                        customer.SocialSecurityNumber ==
                        socialSecurityNumber);

            if (customer is null)
            {
                return Task.FromResult<Customer?>(null);
            }

            customer.AnnualIncome = annualIncome;
            customer.TotalDebt = totalDebt;

            return Task.FromResult<Customer?>(customer);
        }
    }
}