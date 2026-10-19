using LoanApplicationService.Api.Models;
using LoanApplicationService.Api.Repositories;
using System.Text.RegularExpressions;

namespace LoanApplicationService.Api.Services;

public class LoanApplicationService
    : ILoanApplicationService
{
    private readonly ICreditScoreService _creditScoreService;
    private readonly ILoanApplicationRepository _repository;

    public LoanApplicationService(
        ICreditScoreService creditScoreService,
        ILoanApplicationRepository repository)
    {
        _creditScoreService = creditScoreService;
        _repository = repository;
    }

    public async Task<LoanApplication> CreateLoanApplicationAsync(
        string socialSecurityNumber,
        int amount)
    {
        // ============================================
        // Validera personnummer
        // ============================================

        if (string.IsNullOrWhiteSpace(socialSecurityNumber) ||
            !Regex.IsMatch(
                socialSecurityNumber,
                @"^\d{8}-\d{4}$"))
        {
            throw new ArgumentException(
                "Personnumret måste följa formatet YYYYMMDD-XXXX.");
        }


        // ============================================
        // Validera lånebelopp
        // ============================================

        if (amount <= 0)
        {
            throw new ArgumentException(
                "Lånebeloppet måste vara större än 0.");
        }


        // ============================================
        // Hämta kreditvärde
        // ============================================

        int creditScore =
            await _creditScoreService
                .GetCreditScoreAsync(
                    socialSecurityNumber);


        // ============================================
        // Bestäm status
        // ============================================

        LoanApplicationStatus status =
            creditScore >= 50
                ? LoanApplicationStatus.Approved
                : LoanApplicationStatus.Declined;


        // ============================================
        // Skapa LoanApplication
        // ============================================

        var loanApplication =
            new LoanApplication
            {
                SocialSecurityNumber =
                    socialSecurityNumber,

                Amount = amount,

                Status = status
            };


        // ============================================
        // Spara i databasen via repository
        // ============================================

        return await _repository.CreateAsync(
            loanApplication);
    }
}