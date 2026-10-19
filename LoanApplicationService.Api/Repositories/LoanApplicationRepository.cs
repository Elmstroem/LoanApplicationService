using Dapper;
using LoanApplicationService.Api.Models;
using Microsoft.Data.SqlClient;

namespace LoanApplicationService.Api.Repositories;

public class LoanApplicationRepository
    : ILoanApplicationRepository
{
    private readonly IConfiguration _configuration;

    public LoanApplicationRepository(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<LoanApplication> CreateAsync(
        LoanApplication loanApplication)
    {
        string connectionString =
            _configuration.GetConnectionString(
                "DefaultConnection")!;

        await using var connection =
            new SqlConnection(connectionString);

        string sql = """
            INSERT INTO dbo.LoanApplications
            (
                SocialSecurityNumber,
                Amount,
                Status
            )
            OUTPUT
                INSERTED.Id
            VALUES
            (
                @SocialSecurityNumber,
                @Amount,
                @Status
            );
            """;

        int id =
            await connection.QuerySingleAsync<int>(
                sql,
                new
                {
                    loanApplication.SocialSecurityNumber,
                    loanApplication.Amount,
                    Status =
                        loanApplication.Status.ToString()
                });

        loanApplication.Id = id;

        return loanApplication;
    }
}