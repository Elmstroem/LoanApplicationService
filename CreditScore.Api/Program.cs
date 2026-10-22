using CreditScore.Api.Endpoints;
using CreditScore.Api.Repositories;
using CreditScore.Api.Services;

var builder = WebApplication.CreateBuilder(args);


// ============================================
// OpenAPI
// ============================================

builder.Services.AddOpenApi();


// ============================================
// Dependency Injection - Service
// ============================================

builder.Services.AddScoped<
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
        ICustomerRepository,
        CustomerRepository>();
}

// I Production / Azure:
// använd data i minnet
else
{
    builder.Services.AddSingleton<
        ICustomerRepository,
        InMemoryCustomerRepository>();
}


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