using System.Text.Json.Serialization;
using LoanApplicationService.Api.Endpoints;
using LoanApplicationService.Api.Repositories;
using LoanApplicationService.Api.Services;

using LoanAppService =
    LoanApplicationService.Api.Services.LoanApplicationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


// ============================================
// Dependency Injection
// ============================================

builder.Services.AddScoped<
    ILoanApplicationService,
    LoanAppService>();

builder.Services.AddScoped<
    ICreditScoreService,
    CreditScoreService>();

builder.Services.AddScoped<
    ILoanApplicationRepository,
    LoanApplicationRepository>();


// Enum som text i JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapLoanApplicationEndpoints();

app.Run();