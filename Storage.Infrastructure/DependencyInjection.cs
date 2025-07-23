using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage.Application.ItemType.Commands;
using Storage.Application.ItemType.Queries;
using Storage.Infrastructure.Services;
using Storage.Infrastructure.Services.ItemType;

namespace Storage.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IItemTypeCommandService, ItemTypeCommandService>();
        builder.Services.AddScoped<IItemTypeQueryService, IItemTypeQueryService>();

        return builder;
    }
}