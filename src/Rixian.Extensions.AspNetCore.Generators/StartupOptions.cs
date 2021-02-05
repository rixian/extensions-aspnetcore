// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Generators
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal class.")]
    internal class StartupOptions
    {
        public bool EnableWebApi { get; set; }

        public bool EnableRazorPages { get; set; }

        public bool EnableDataProtection { get; set; }

        public bool EnableRedis { get; set; }

        public bool EnableOAuth2 { get; set; }

        public bool EnableOpenIdConnect { get; set; }
    }
}
