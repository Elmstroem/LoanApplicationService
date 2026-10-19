using LoanApplicationService.Api.Models;

namespace LoanApplicationService.Api.Repositories;

public class InMemoryLoanApplicationRepository
    : ILoanApplicationRepository
{
    // Här lagras alla låneansökningar i minnet
    private readonly List<LoanApplication> _loanApplications = [];

    // Används för att skapa unika Id:n
    private int _nextId = 1;

    // Skyddar listan och Id-räknaren om flera requests kommer samtidigt
    private readonly object _lock = new();


    public Task<LoanApplication> CreateAsync(
        LoanApplication loanApplication)
    {
        lock (_lock)
        {
            loanApplication.Id = _nextId;

            _nextId++;

            _loanApplications.Add(loanApplication);
        }

        return Task.FromResult(loanApplication);
    }
}