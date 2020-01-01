// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.StackExchangeRedis
{
    using System.Collections.Generic;
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Configuration class for Redis.
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// Gets or sets the Redis configuration information, a.k.a. the connection string.
        /// </summary>
        public string? Configuration { get; set; }

        /// <summary>
        /// Gets or sets the instance name of the service connecting to Redis. Optional.
        /// </summary>
        public string? InstanceName { get; set; }

        /// <summary>
        /// Checks if all the required configuration values are present.
        /// </summary>
        /// <returns>An optional error result or nothing.</returns>
        public Result CheckRequiredValues()
        {
            List<ErrorBase>? errors = null;

            if (string.IsNullOrWhiteSpace(this.Configuration))
            {
                errors ??= new List<ErrorBase>();
                errors.Add(new MissingRequiredConfigurationFieldError(nameof(this.Configuration)));
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
