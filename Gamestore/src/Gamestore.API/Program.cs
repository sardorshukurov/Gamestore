using Gamestore.API.Middlewares;
using Gamestore.BLL;
using Gamestore.DAL;
using Gamestore.DAL.Data;
using Gamestore.DAL.Data.Seeder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddTransient<AddTotalGamesInHeaderMiddleware>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddBusinessLogicServices();

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

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<MainDbContext>();
    DbSeeder.AddDemoData(context);
}

app.Run();