using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage.Application.Interfaces;
using Storage.Infrastructure.Services;

namespace Storage.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IItemTypeRepository, ItemTypeRepository>();

        return builder;
    }
}