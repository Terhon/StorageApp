using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage.Application.Commands;
using Storage.Infrastructure.Services;

namespace Storage.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IItemTypeCommandService, ItemTypeCommandService>();

        return builder;
    }
}