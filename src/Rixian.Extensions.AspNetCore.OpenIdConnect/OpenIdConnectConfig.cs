// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Configuration class for the OpenID Connect configuration.
    /// </summary>
    public class OpenIdConnectConfig
    {
        /// <summary>
        /// Gets or sets the OpenID Connect authority.
        /// </summary>
        public string? Authority { get; set; }

        /// <summary>
        /// Gets or sets the optional custom health endpoint.
        /// </summary>
        public string? AuthorityHealthEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the api configuration.
        /// </summary>
        public OpenIdConnectApiConfig? Api { get; set; }

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
        public Result CheckRequiredValues()
        {
            List<ErrorBase>? errors = null;

            if (string.IsNullOrWhiteSpace(this.Authority))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.Authority)));
            }

            if (this.Api == null)
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationSectionError(nameof(this.Api)));
            }
            else
            {
                Result isApiValid = this.Api.CheckRequiredValues();
                if (isApiValid.IsError)
                {
                    errors ??= new List<ErrorBase>();
                    errors.Add(isApiValid.Error);
                }
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
