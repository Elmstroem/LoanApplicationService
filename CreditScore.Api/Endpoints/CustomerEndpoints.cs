using CreditScore.Api.Dtos;
using CreditScore.Api.Services;

namespace CreditScore.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(
        this WebApplication app)
    {
        // ============================================
        // GET - Hämta Credit Score
        // ============================================

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


        // ============================================
        // POST - Skapa kund + Credit Score
        // ============================================

        app.MapPost(
            "/api/customers",
            async (
                CreateCustomerDto request,
                ICreditScoreService creditScoreService) =>
            {
                try
                {
                    int creditScore =
                        await creditScoreService
                            .CreateCreditScoreAsync(
                                request.SocialSecurityNumber,
                                request.Name,
                                request.AnnualIncome,
                                request.TotalDebt);

                    var result = new CreditScoreDto
                    {
                        SocialSecurityNumber =
                            request.SocialSecurityNumber,

                        CreditScore =
                            creditScore
                    };

                    return Results.Created(
                        $"/api/customers/{request.SocialSecurityNumber}/credit-score",
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


        // ============================================
        // PUT - Uppdatera kund + Credit Score
        // ============================================

        app.MapPut(
            "/api/customers/{socialSecurityNumber}",
            async (
                string socialSecurityNumber,
                UpdateCustomerDto request,
                ICreditScoreService creditScoreService) =>
            {
                int? creditScore =
                    await creditScoreService
                        .UpdateCreditScoreAsync(
                            socialSecurityNumber,
                            request.AnnualIncome,
                            request.TotalDebt);

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