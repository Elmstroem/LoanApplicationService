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


    // ============================================
    // GET - Hämta kund via personnummer
    // ============================================

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


    // ============================================
    // CREATE - Skapa ny kund
    // ============================================

    public async Task<Customer> CreateAsync(
        Customer customer)
    {
        string connectionString =
            _configuration.GetConnectionString(
                "DefaultConnection")!;

        await using var connection =
            new SqlConnection(connectionString);

        string sql = """
            INSERT INTO dbo.Customers
            (
                SocialSecurityNumber,
                Name,
                AnnualIncome,
                TotalDebt
            )
            OUTPUT INSERTED.Id
            VALUES
            (
                @SocialSecurityNumber,
                @Name,
                @AnnualIncome,
                @TotalDebt
            );
            """;

        int id =
            await connection.QuerySingleAsync<int>(
                sql,
                new
                {
                    customer.SocialSecurityNumber,
                    customer.Name,
                    customer.AnnualIncome,
                    customer.TotalDebt
                });

        customer.Id = id;

        return customer;
    }


    // ============================================
    // UPDATE - Uppdatera kund
    // ============================================

    public async Task<Customer?> UpdateAsync(
        string socialSecurityNumber,
        decimal annualIncome,
        decimal totalDebt)
    {
        string connectionString =
            _configuration.GetConnectionString(
                "DefaultConnection")!;

        await using var connection =
            new SqlConnection(connectionString);

        string sql = """
            UPDATE dbo.Customers
            SET
                AnnualIncome = @AnnualIncome,
                TotalDebt = @TotalDebt
            OUTPUT
                INSERTED.Id,
                INSERTED.SocialSecurityNumber,
                INSERTED.Name,
                INSERTED.AnnualIncome,
                INSERTED.TotalDebt
            WHERE SocialSecurityNumber = @SocialSecurityNumber;
            """;

        Customer? updatedCustomer =
            await connection.QuerySingleOrDefaultAsync<Customer>(
                sql,
                new
                {
                    SocialSecurityNumber =
                        socialSecurityNumber,
                    AnnualIncome =
                        annualIncome,
                    TotalDebt =
                        totalDebt
                });

        return updatedCustomer;
    }
}