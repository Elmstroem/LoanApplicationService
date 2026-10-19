namespace LoanApplicationService.Api.Dtos;

public class CreateLoanApplicationDto
{
    public string SocialSecurityNumber { get; set; } = "";

    public int Amount { get; set; }
}