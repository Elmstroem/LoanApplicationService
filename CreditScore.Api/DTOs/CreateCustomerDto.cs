namespace CreditScore.Api.Dtos;

public class CreateCustomerDto
{
    public required string SocialSecurityNumber { get; set; }

    public required string Name { get; set; }

    public decimal AnnualIncome { get; set; }

    public decimal TotalDebt { get; set; }
}