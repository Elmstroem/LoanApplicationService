using LoanApplicationService.Api.Models;

namespace LoanApplicationService.Api.Dtos;

public class LoanApplicationResultDto
{
    public int Id { get; set; }

    public LoanApplicationStatus Status { get; set; }
}