// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// Error used when a configuration section is missing.
    /// </summary>
    public class MissingRequiredConfigurationSectionError : ErrorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingRequiredConfigurationSectionError"/> class.
        /// </summary>
        /// <param name="sectionPath">The path to the section.</param>
        public MissingRequiredConfigurationSectionError(string sectionPath)
        {
            this.Code = "MissingRequiredConfiguratioSection";
            this.Message = Properties.Resources.MissingRequiredConfigurationSectionErrorMessage;
            this.Target = sectionPath;
        }
    }
}
