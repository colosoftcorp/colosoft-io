using Colosoft.IO.Storage.Blazor;
using Colosoft.IO.Storage.Blazor.JsonConverters;
using Colosoft.IO.Storage.Blazor.Serialization;
using Colosoft.IO.Storage.Blazor.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Colosoft.IO.Storage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorLocalStorage(this IServiceCollection services)
           => AddBlazorLocalStorage(services, null);

        public static IServiceCollection AddBlazorLocalStorage(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            return services
                .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                .AddSingleton<IStorageProvider, BrowserStorageProvider>()
                .AddSingleton<ISyncStorage, Blazor.Storage>()
                .AddSingleton(serviceProvider => (IStorage)serviceProvider.GetRequiredService<ISyncStorage>())
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}
