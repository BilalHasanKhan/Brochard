﻿using System;
using Microsoft.Framework.Localization;

namespace Orchard.Abstractions {
    public class OrchardCoreException : Exception {
        private readonly LocalizedString _localizedMessage;

        public OrchardCoreException(LocalizedString message)
            : base(message) {
            _localizedMessage = message;
        }

        public OrchardCoreException(LocalizedString message, Exception innerException)
            : base(message, innerException) {
            _localizedMessage = message;
        }

        public LocalizedString LocalizedMessage { get { return _localizedMessage; } }
    }
}