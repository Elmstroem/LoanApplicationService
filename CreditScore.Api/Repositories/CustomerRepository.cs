using CreditScore.Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CreditScore.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IConfiguration _configuration;

    public CustomerRepository(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Customer?> GetBySocialSecurityNumberAsync(
        string socialSecurityNumber)
    {
        string connectionString =
            _configuration.GetConnectionString(
                "DefaultConnection")!;

        await using var connection =
            new SqlConnection(connectionString);

        string sql = """
            SELECT
                Id,
                SocialSecurityNumber,
                Name,
                AnnualIncome,
                TotalDebt
            FROM dbo.Customers
            WHERE SocialSecurityNumber = @SocialSecurityNumber;
            """;

        Customer? customer =
            await connection.QuerySingleOrDefaultAsync<Customer>(
                sql,
                new
                {
                    SocialSecurityNumber =
                        socialSecurityNumber
                });

        return customer;
    }
}