// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Generators
{
    using System.IO;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal class.")]
    internal static class Sources
    {
        public static string GetResourceText(string name)
        {
            Assembly? assembly = typeof(Rixian.Extensions.AspNetCore.Generators.Sources).GetTypeInfo().Assembly;
            using Stream? resource = assembly.GetManifestResourceStream(name);
            if (resource != null)
            {
                using StreamReader sr = new StreamReader(resource);
                return sr.ReadToEnd();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string DataProtectionConfig() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.DataProtectionConfig.cs");

        public static string KeyRingConfig() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.KeyRingConfig.cs");

        public static string ApiConfig() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.ApiConfig.cs");

        public static string OAuth2Config() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.OAuth2Config.cs");

        public static string OpenIdConnectConfig() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.OpenIdConnectConfig.cs");

        public static string RedisConfig() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.RedisConfig.cs");

        public static string SlugifyParameterTransformer() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.SlugifyParameterTransformer.cs");

        public static string MissingRequiredConfigurationFieldError() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.MissingRequiredConfigurationFieldError.cs");

        public static string MissingRequiredConfigurationSectionError() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.MissingRequiredConfigurationSectionError.cs");

        public static string InvalidConfigurationError() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.InvalidConfigurationError.cs");

        public static string DataProtectionServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.DataProtectionServiceCollectionExtensions.cs");

        public static string ApplicationBuilderExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.ApplicationBuilderExtensions.cs");

        public static string ServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.ServiceCollectionExtensions.cs");

        public static string SameSiteCookiesServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.SameSiteCookiesServiceCollectionExtensions.cs");

        public static string AuthInfoHelper() => GetResourceText("Rixian.Extensions.AspNetCore.Generators.source.AuthInfoHelper.cs");
    }
}
