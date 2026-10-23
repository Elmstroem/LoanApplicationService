using System.Text.Json.Serialization;
using LoanApplicationService.Api.Endpoints;
using LoanApplicationService.Api.Repositories;
using LoanApplicationService.Api.Services;

using LoanAppService =
    LoanApplicationService.Api.Services.LoanApplicationService;

var builder = WebApplication.CreateBuilder(args);


// ============================================
// OpenAPI
// ============================================

builder.Services.AddOpenApi();


// ============================================
// Dependency Injection - Loan Application Service
// ============================================

builder.Services.AddScoped<
    ILoanApplicationService,
    LoanAppService>();


// ============================================
// Dependency Injection - Credit Score HTTP Client
// ============================================

builder.Services.AddHttpClient<
    ICreditScoreService,
    CreditScoreService>();


// ============================================
// Dependency Injection - Repository
// ============================================

// Lokalt i Development:
// använd SQL Server + Dapper
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<
        ILoanApplicationRepository,
        LoanApplicationRepository>();
}

// I Production / Azure:
// använd InMemory repository
else
{
    builder.Services.AddSingleton<
        ILoanApplicationRepository,
        InMemoryLoanApplicationRepository>();
}


// ============================================
// JSON-inställningar
// ============================================

// Returnera enum som exempelvis "Approved"
// istället för 0
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});


var app = builder.Build();


// ============================================
// HTTP Pipeline
// ============================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


// ============================================
// Endpoints
// ============================================

app.MapLoanApplicationEndpoints();


app.Run();