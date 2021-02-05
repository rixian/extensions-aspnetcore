// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

#nullable enable
namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Error used when configuration is invalid.
    /// </summary>
    public class InvalidConfigurationError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidConfigurationError"/> class.
        /// </summary>
        public InvalidConfigurationError()
        {
            this.Code = "InvalidConfiguration";
            this.Message = "Invalid configuration provided.";
        }
    }
}
