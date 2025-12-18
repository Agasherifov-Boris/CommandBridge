using CommandBridge.Tests.DemoProjects.Shop.Entities;
using CommandBridge.Tests.DemoProjects.Shop.Interfaces;
using CommandBridge.Tests.DemoProjects.Shop.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CommandBridge.Tests.DemoProjects.Shop.Extensions;

public static class DIExtensions
{
    public static void AddShopServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Client>, ClientRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();

    }
}