using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess.Repositories.Users;
using CashFlow.Infrastructure.Security.Cryptography;
using CashFlow.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddExpensesRepositories(services);
        AddUsersRepositories(services);
        AddDbContext(services, configuration);
        AddUnitOfWork(services);
        AddToken(services, configuration);
        AddCryptography(services);
    }

    private static void AddExpensesRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesReadRepository, ExpensesRepository>();
        services.AddScoped<IExpensesWriteRepository, ExpensesRepository>();
        services.AddScoped<IExpensesUpdateRepository, ExpensesRepository>();
        services.AddScoped<IExpensesDeleteRepository, ExpensesRepository>();
    }

    private static void AddUsersRepositories(IServiceCollection services)
    {
        services.AddScoped<IUsersReadRepository, UsersRepository>();
        services.AddScoped<IUsersWriteRepository, UsersRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<CashFlowDbContext>(
            config => config.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
        );
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        uint expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        string? signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }

    private static void AddCryptography(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, Cryptography>();
    }
}
