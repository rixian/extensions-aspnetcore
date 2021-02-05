// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

#nullable enable
namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Error used when a configuration section is missing.
    /// </summary>
    public class MissingRequiredConfigurationSectionError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingRequiredConfigurationSectionError"/> class.
        /// </summary>
        /// <param name="sectionPath">The path to the section.</param>
        public MissingRequiredConfigurationSectionError(string sectionPath)
        {
            this.Code = "MissingRequiredConfiguratioSection";
            this.Message = "A required configuration section missing.";
            this.Target = sectionPath;
        }
    }
}
