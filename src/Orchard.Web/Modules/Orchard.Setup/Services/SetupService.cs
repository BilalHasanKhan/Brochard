using Orchard.Hosting;
using Orchard.Hosting.Descriptor.Models;
using Orchard.Hosting.Extensions;
using Orchard.Hosting.ShellBuilders;
using System;
using System.Linq;
using Microsoft.AspNet.Http;
using Orchard.Abstractions.Localization;
using Orchard.Configuration.Environment;
using Orchard.Environment.Extensions;

namespace Orchard.Setup.Services {
    public class SetupService : ISetupService {
        private readonly IOrchardHost _orchardHost;
        private readonly IShellSettingsManager _shellSettingsManager;
        private readonly IShellContainerFactory _shellContainerFactory;
        private readonly ICompositionStrategy _compositionStrategy;
        private readonly IExtensionManager _extensionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SetupService(
            IOrchardHost orchardHost,
            IShellSettingsManager shellSettingsManager,
            IShellContainerFactory shellContainerFactory,
            ICompositionStrategy compositionStrategy,
            IExtensionManager extensionManager,
            IHttpContextAccessor httpContextAccessor) {
            _orchardHost = orchardHost;
            _shellSettingsManager = shellSettingsManager;
            _shellContainerFactory = shellContainerFactory;
            _compositionStrategy = compositionStrategy;
            _extensionManager = extensionManager;
            _httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public string Setup(SetupContext context) {
            string executionId = Guid.NewGuid().ToString();

            // The vanilla Orchard distibution has the following features enabled.
            string[] hardcoded = {
                // Framework
                "Orchard.Hosting",
                // Core
                "Settings",
                // Test Modules
                "Orchard.Demo", "Orchard.Test1"
                };

            context.EnabledFeatures = hardcoded.Union(context.EnabledFeatures ?? Enumerable.Empty<string>()).Distinct().ToList();

            var shellSettings = new ShellSettings();

            //if (shellSettings.DataProviders.Any()) {
            //    DataProvider provider = new DataProvider();
                //shellSettings.DataProvider = context.DatabaseProvider;
                //shellSettings.DataConnectionString = context.DatabaseConnectionString;
                //shellSettings.DataTablePrefix = context.DatabaseTablePrefix;
            //}

            // TODO: Add Encryption Settings in

            var shellDescriptor = new ShellDescriptor {
                Features = context.EnabledFeatures.Select(name => new ShellFeature { Name = name })
            };

            // creating a standalone environment. 
            // in theory this environment can be used to resolve any normal components by interface, and those
            // components will exist entirely in isolation - no crossover between the safemode container currently in effect

            // must mark state as Running - otherwise standalone enviro is created "for setup"
            shellSettings.State = TenantState.Running;

            // TODO: Remove and mirror Orchard Setup
            shellSettings.RequestUrlHost = _httpContextAccessor.HttpContext.Request.Host.Value;
            //shellSettings.DataProvider = "InMemory";

            _shellSettingsManager.SaveSettings(shellSettings);

            return executionId;
        }
    }
}