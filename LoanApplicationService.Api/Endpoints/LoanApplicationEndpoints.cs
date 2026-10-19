using LoanApplicationService.Api.Dtos;
using LoanApplicationService.Api.Services;

namespace LoanApplicationService.Api.Endpoints;

public static class LoanApplicationEndpoints
{
    public static void MapLoanApplicationEndpoints(
        this WebApplication app)
    {
        app.MapPost("/api/loanapplications", async (
            CreateLoanApplicationDto request,
            ILoanApplicationService loanApplicationService) =>
        {
            try
            {
                var loanApplication =
                    await loanApplicationService
                        .CreateLoanApplicationAsync(
                            request.SocialSecurityNumber,
                            request.Amount);

                var result = new LoanApplicationResultDto
                {
                    Id = loanApplication.Id,
                    Status = loanApplication.Status
                };

                return Results.Created(
                    $"/api/loanapplications/{loanApplication.Id}",
                    result);
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(
                    new
                    {
                        error = exception.Message
                    });
            }
        });
    }
}