namespace LoanApplicationService.Api.Dtos;

public class CreditScoreResponseDto
{
    public required string SocialSecurityNumber { get; set; }

    public int CreditScore { get; set; }
}