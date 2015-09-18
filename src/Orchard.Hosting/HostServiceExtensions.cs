using System;
using Microsoft.Dnx.Compilation.Caching;
using Microsoft.Framework.DependencyInjection;
using Orchard.FileSystem;
using Orchard.Configuration.Environment;
using Orchard.Hosting.ShellBuilders;
using Orchard.DependencyInjection;

namespace Orchard.Hosting {
    public static class HostServiceExtensions {
        public static IServiceCollection AddHost(
            [NotNull] this IServiceCollection services, Action<IServiceCollection> additionalDependencies) {

            services.AddFileSystems();

            // Caching - Move out
            services.AddInstance<ICacheContextAccessor>(new CacheContextAccessor());
            services.AddSingleton<ICache, Cache>();

            additionalDependencies(services);
            
            services.AddTransient<IOrchardShellHost, DefaultOrchardShellHost>();

            return services.AddFallback();
        }

        public static IServiceCollection AddHostCore(this IServiceCollection services) {
            services.AddSingleton<IOrchardHost, DefaultOrchardHost>();
            {
                services.AddSingleton<IShellSettingsManager, ShellSettingsManager>();

                services.AddSingleton<IShellContextFactory, ShellContextFactory>();
                {
                    services.AddSingleton<ICompositionStrategy, CompositionStrategy>();
                    {
                        services.AddSingleton<IOrchardLibraryManager, OrchardLibraryManager>();
                    }

                    services.AddSingleton<IShellContainerFactory, ShellContainerFactory>();
                }
            }

            return services;
        }
    }
}