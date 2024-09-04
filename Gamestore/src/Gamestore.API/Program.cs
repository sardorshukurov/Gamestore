using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Gamestore.API.Middlewares;
using Gamestore.BLL;
using Gamestore.Common.Helpers;
using Gamestore.DAL;
using Gamestore.DAL.Data;
using Gamestore.DAL.Data.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// configuring logging
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

// adding payment microservice
builder.Services.AddHttpClient("PaymentAPI", client =>
{
    var uriString = builder.Configuration.GetSection("PaymentMicroservice").Value;

    if (!string.IsNullOrEmpty(uriString))
    {
        client.BaseAddress = new Uri(uriString);
    }
});

// adding authorization microservice
builder.Services.AddHttpClient("AuthAPI", client =>
{
    var uriString = builder.Configuration.GetSection("AuthMicroservice").Value;

    if (!string.IsNullOrEmpty(uriString))
    {
        client.BaseAddress = new Uri(uriString);
    }
});

// adding cache
builder.Services.AddMemoryCache();

// adding controllers, fluent validation, endpoints, and swagger
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

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

// configuring totla games middleware
builder.Services.Configure<TotalGamesMiddlewareConfig>(builder.Configuration.GetSection("TotalGamesMiddleware"));

// adding authentication and authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtKey").Value!)),
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging(o =>
    o.MessageTemplate = "Processed {RequestPath} in {Elapsed:0.0000} ms Response {StatusCode}");

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseMiddleware<AddTotalGamesInHeaderMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// adding demo data if needed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
    DbSeeder.AddDemoData(context);
}

app.Run();