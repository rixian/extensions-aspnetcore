// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

#nullable enable
namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Configuration class for the api.
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// Gets or sets a value that defines the default version for the Api.
        /// </summary>
        public string? DefaultVersion { get; set; }

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
#pragma warning disable CA1822 // Mark members as static
        public Result CheckRequiredValues()
#pragma warning restore CA1822 // Mark members as static
        {
            // Nothing is required.
            return Result.Default;
        }

        /// <summary>
        /// Ensures that all the required configuration values are present. Throws an <see cref="ErrorException"/> if not.
        /// </summary>
        public void EnsureRequiredValues()
        {
            Result isValid = this.CheckRequiredValues();
            if (isValid.IsFail)
            {
                throw new ErrorException(isValid.Error);
            }
        }
    }
}
