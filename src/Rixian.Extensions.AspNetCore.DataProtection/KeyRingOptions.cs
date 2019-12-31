// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    /// <summary>
    /// Configuration class for the DataProtection Key Ring.
    /// </summary>
    public class KeyRingOptions
    {
        /// <summary>
        /// Gets or sets the key name.
        /// </summary>
        public string? KeyName { get; set; }

        /// <summary>
        /// Gets or sets the key identifier.
        /// </summary>
        public string? KeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the client Id used to access KeyVault.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret used to access KeyVault.
        /// </summary>
        public string? ClientSecret { get; set; }
    }
}
