// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.DataProtection
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Configuration class for the DataProtection Key Ring.
    /// </summary>
    public class KeyRingConfig
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

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
        public Result CheckRequiredValues()
        {
            List<ErrorBase>? errors = null;

            if (string.IsNullOrWhiteSpace(this.KeyName))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.KeyName)));
            }

            if (string.IsNullOrWhiteSpace(this.KeyIdentifier))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.KeyIdentifier)));
            }

            if (string.IsNullOrWhiteSpace(this.ClientId))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.ClientId)));
            }

            if (string.IsNullOrWhiteSpace(this.ClientSecret))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.ClientSecret)));
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
