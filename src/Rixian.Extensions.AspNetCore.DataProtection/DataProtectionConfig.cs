﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;

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
        public KeyRingConfig? KeyRing { get; set; }

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
        public Result CheckRequiredValues()
        {
            List<ErrorBase>? errors = null;

            if (string.IsNullOrWhiteSpace(this.ApplicationDiscriminator))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.ApplicationDiscriminator)));
            }

            if (string.IsNullOrWhiteSpace(this.AzureStorage))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.AzureStorage)));
            }

            if (this.KeyRing == null)
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationSectionError(nameof(this.KeyRing)));
            }

            if (errors != null)
            {
                return new InvalidConfigurationError
                {
                    Details = errors,
                };
            }

            return Result.Default;
        }

        /// <summary>
        /// Ensures that all the required configuration values are present. Throws an <see cref="ErrorException"/> if not.
        /// </summary>
        public void EnsureRequiredValues()
        {
            Result isValid = this.CheckRequiredValues();
            if (isValid.IsError)
            {
                throw new ErrorException(isValid.Error);
            }
        }
    }
}
