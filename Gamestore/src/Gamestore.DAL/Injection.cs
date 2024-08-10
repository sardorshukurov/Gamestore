using Gamestore.DAL.Data;
using Gamestore.DAL.Repository;
using Gamestore.DAL.Settings;
using Gamestore.Domain.Common;
using Gamestore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Gamestore.DAL;

public static class Injection
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Game>, Repository<Game>>();
        services.AddScoped<IRepository<Genre>, Repository<Genre>>();
        services.AddScoped<IRepository<Platform>, Repository<Platform>>();
        services.AddScoped<IRepository<GameGenre>, Repository<GameGenre>>();
        services.AddScoped<IRepository<GamePlatform>, Repository<GamePlatform>>();

        services.AddScoped<IRepository<Publisher>, Repository<Publisher>>();

        services.AddScoped<IRepository<Order>, Repository<Order>>();
        services.AddScoped<IRepository<OrderGame>, Repository<OrderGame>>();
        services.AddScoped<IRepository<PaymentMethod>, Repository<PaymentMethod>>();

        services.AddScoped<IRepository<Comment>, Repository<Comment>>();
        services.AddScoped<IRepository<Ban>, Repository<Ban>>();

        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, string serviceName)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : BaseMongoEntity
    {
        services.AddScoped<IMongoRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database!, collectionName);
        });

        return services;
    }
}