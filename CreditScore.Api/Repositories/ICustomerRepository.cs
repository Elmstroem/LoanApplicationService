using CreditScore.Api.Models;

namespace CreditScore.Api.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetBySocialSecurityNumberAsync(
        string socialSecurityNumber);

    Task<Customer> CreateAsync(
        Customer customer);

    Task<Customer?> UpdateAsync(
        string socialSecurityNumber,
        decimal annualIncome,
        decimal totalDebt);
}