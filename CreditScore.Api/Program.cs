using CreditScore.Api.Endpoints;
using CreditScore.Api.Repositories;
using CreditScore.Api.Services;

var builder = WebApplication.CreateBuilder(args);


// ============================================
// OpenAPI
// ============================================

builder.Services.AddOpenApi();


// ============================================
// Dependency Injection
// ============================================

builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    ICreditScoreService,
    CreditScoreService>();


var app = builder.Build();


// ============================================
// HTTP pipeline
// ============================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


// ============================================
// Endpoints
// ============================================

app.MapCustomerEndpoints();


app.Run();