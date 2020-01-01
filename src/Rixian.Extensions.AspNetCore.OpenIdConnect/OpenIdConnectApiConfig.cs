// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Configuration class for the OpenID Connect api configuration.
    /// </summary>
    public class OpenIdConnectApiConfig
    {
        /// <summary>
        /// Gets or sets the api name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the api secret.
        /// </summary>
        public string? Secret { get; set; }

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
        public Result CheckRequiredValues()
        {
            List<ErrorBase>? errors = null;

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.Name)));
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
