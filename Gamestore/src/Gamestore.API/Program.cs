using FluentValidation.AspNetCore;
using Gamestore.API.Middlewares;
using Gamestore.BLL;
using Gamestore.BLL.Converters.GameFilterEnums;
using Gamestore.Common.Helpers;
using Gamestore.DAL;
using Gamestore.DAL.Data;
using Gamestore.DAL.Data.Seeder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// configuring logging
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddHttpClient("PaymentAPI", client =>
{
    var uriString = builder.Configuration.GetSection("PaymentMicroservice").Value;

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
        o.JsonSerializerOptions.Converters.Add(new DateFilterOptionConverter());
        o.JsonSerializerOptions.Converters.Add(new PaginationOptionConverter());
        o.JsonSerializerOptions.Converters.Add(new SortingOptionConverter());
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

builder.Services.Configure<TotalGamesMiddlewareConfig>(builder.Configuration.GetSection("TotalGamesMiddleware"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(o =>
    o.MessageTemplate = "Processed {RequestPath} in {Elapsed:0.0000} ms Response {StatusCode}");

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<AddTotalGamesInHeaderMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapControllers();

// adding demo data if needed
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
    DbSeeder.AddDemoData(context);
}

app.Run();