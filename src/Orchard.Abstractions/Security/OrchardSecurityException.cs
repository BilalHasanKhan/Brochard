﻿using System;
using Microsoft.Framework.Localization;
using Orchard.Abstractions;

namespace Orchard.Security {
    public class OrchardSecurityException : OrchardCoreException {
        public OrchardSecurityException(LocalizedString message) : base(message) { }
        public OrchardSecurityException(LocalizedString message, Exception innerException) : base(message, innerException) { }

        public string PermissionName { get; set; }
    }
}
