using FluentValidation.AspNetCore;
using Gamestore.API.Middlewares;
using Gamestore.BLL;
using Gamestore.Common.Helpers;
using Gamestore.DAL;
using Gamestore.DAL.Data;
using Gamestore.DAL.Data.Seeder;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// configuring logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File(
        "Logs/exceptions-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Host.UseSerilog();

// adding middlewares
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddTransient<AddTotalGamesInHeaderMiddleware>();
builder.Services.AddTransient<RequestLoggingMiddleware>();

// adding cache
builder.Services.AddMemoryCache();

// adding controllers, fluent validation, endpoints, and swagger
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding dbContext, repositories and bll services
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddBusinessLogicServices();

// adding validators
builder.Services.RegisterValidators();

// adding cors configurations
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("x-total-number-of-games"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<AddTotalGamesInHeaderMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

// adding demo data if needed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
    DbSeeder.AddDemoData(context);
}

app.Run();