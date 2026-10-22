using CreditScore.Api.Dtos;
using CreditScore.Api.Services;

namespace CreditScore.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(
        this WebApplication app)
    {
        app.MapGet(
            "/api/customers/{socialSecurityNumber}/credit-score",
            async (
                string socialSecurityNumber,
                ICreditScoreService creditScoreService) =>
            {
                int? creditScore =
                    await creditScoreService
                        .GetCreditScoreAsync(
                            socialSecurityNumber);

                if (creditScore is null)
                {
                    return Results.NotFound(
                        new
                        {
                            error =
                                $"Kunden med personnummer {socialSecurityNumber} finns inte."
                        });
                }

                var result = new CreditScoreDto
                {
                    SocialSecurityNumber =
                        socialSecurityNumber,

                    CreditScore =
                        creditScore.Value
                };

                return Results.Ok(result);
            });
    }
}