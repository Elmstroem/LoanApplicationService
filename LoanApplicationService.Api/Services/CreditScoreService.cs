namespace LoanApplicationService.Api.Services;

public class CreditScoreService : ICreditScoreService
{
    public async Task<int> GetCreditScoreAsync(
        string socialSecurityNumber)
    {
        // Simulerar ett anrop till en extern kreditupplysningstjänst
        await Task.Delay(500);

        // Tillfälligt påhittat kreditvärde
        return 50;
    }
}