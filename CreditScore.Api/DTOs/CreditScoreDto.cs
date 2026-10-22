namespace CreditScore.Api.Dtos;

public class CreditScoreDto
{
    public required string SocialSecurityNumber { get; set; }

    public int CreditScore { get; set; }
}