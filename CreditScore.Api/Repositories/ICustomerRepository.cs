using CreditScore.Api.Models;

namespace CreditScore.Api.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetBySocialSecurityNumberAsync(
        string socialSecurityNumber);
}