﻿using Microsoft.Framework.DependencyInjection;
using Orchard.DependencyInjection;
using Orchard.Hosting;
using Orchard.Hosting.Web.Routing.Routes;
using Orchard.Web;

namespace Orchard.Setup {
    public class SetupMode : IModule {
        public void Configure(IServiceCollection serviceCollection) {
            new ShellModule().Configure(serviceCollection);

            serviceCollection.AddScoped<IRoutePublisher, RoutePublisher>();
        }
    }
}
