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
}