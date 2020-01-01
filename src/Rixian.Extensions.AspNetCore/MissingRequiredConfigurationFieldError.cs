// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Error used when a configuration field is missing.
    /// </summary>
    public class MissingRequiredConfigurationFieldError : ErrorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingRequiredConfigurationFieldError"/> class.
        /// </summary>
        /// <param name="fieldPath">The path to the field.</param>
        public MissingRequiredConfigurationFieldError(string fieldPath)
        {
            this.Code = "MissingRequiredConfigurationField";
            this.Message = Properties.Resources.MissingRequiredConfigurationFieldErrorMessage;
            this.Target = fieldPath;
        }
    }
}
