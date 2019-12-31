// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    /// <summary>
    /// Configuration class for DataProtection.
    /// </summary>
    public class DataProtectionConfig
    {
        /// <summary>
        /// Gets or sets the application discriminator.
        /// </summary>
        public string? ApplicationDiscriminator { get; set; }

        /// <summary>
        /// Gets or sets the Azure Storage connection string.
        /// </summary>
        public string? AzureStorage { get; set; }

        /// <summary>
        /// Gets or sets the data protection key ring values.
        /// </summary>
        public KeyRingOptions? KeyRing { get; set; }
    }
}
