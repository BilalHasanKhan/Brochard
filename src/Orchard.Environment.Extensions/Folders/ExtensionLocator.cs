﻿using System.Collections.Generic;
using System.Linq;
using Orchard.Environment.Extensions.Models;
using Microsoft.Framework.OptionsModel;

namespace Orchard.Environment.Extensions.Folders {
    public class ExtensionLocator : IExtensionLocator {
        private readonly IOptions<ExtensionHarvestingOptions> _optionsAccessor;
        private readonly IExtensionHarvester _extensionHarvester;

        public ExtensionLocator(
            [NotNull] IOptions<ExtensionHarvestingOptions> optionsAccessor,
            [NotNull] IExtensionHarvester extensionHarvester) {
            _optionsAccessor = optionsAccessor;
            _extensionHarvester = extensionHarvester;
        }

        public IEnumerable<ExtensionDescriptor> AvailableExtensions() {
            return _optionsAccessor.Value.ModuleLocationExpanders
                .SelectMany(x => _extensionHarvester.HarvestExtensions(
                    x.SearchPaths, x.ExtensionType, x.ManifestName, x.ManifestOptional));
        }
    }
}