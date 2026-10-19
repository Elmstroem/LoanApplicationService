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
// Dependency Injection - Services
// ============================================

builder.Services.AddScoped<
    ILoanApplicationService,
    LoanAppService>();

builder.Services.AddScoped<
    ICreditScoreService,
    CreditScoreService>();


// ============================================
// Dependency Injection - Repository
// ============================================

// Lokalt i Development:
// använd riktig SQL Server + Dapper
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<
        ILoanApplicationRepository,
        LoanApplicationRepository>();
}

// I Production / Azure:
// använd repository som lagrar data i minnet
else
{
    builder.Services.AddSingleton<
        ILoanApplicationRepository,
        InMemoryLoanApplicationRepository>();
}


// ============================================
// JSON-inställningar
// ============================================

// Gör så att enum returneras som exempelvis
// "Approved" istället för 0
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});


var app = builder.Build();


// ============================================
// HTTP request pipeline
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