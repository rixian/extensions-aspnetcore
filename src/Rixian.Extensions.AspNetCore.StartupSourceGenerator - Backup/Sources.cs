// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.StartupSourceGenerator
{
    using System.IO;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    internal static class Sources
    {
        public static string GetResourceText(string name)
        {
            var assembly = typeof(Rixian.Extensions.AspNetCore.StartupSourceGenerator.Sources).GetTypeInfo().Assembly;
            using Stream resource = assembly.GetManifestResourceStream(name);
            using StreamReader sr = new StreamReader(resource);
            return sr.ReadToEnd();
        }

        public static string DataProtectionConfig() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.DataProtectionConfig.cs");
        public static string KeyRingConfig() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.KeyRingConfig.cs");
        public static string ApiConfig() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.ApiConfig.cs");
        public static string OAuth2Config() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.OAuth2Config.cs");
        public static string OpenIdConnectConfig() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.OpenIdConnectConfig.cs");
        public static string RedisConfig() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.RedisConfig.cs");
        public static string SlugifyParameterTransformer() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.SlugifyParameterTransformer.cs");

        public static string MissingRequiredConfigurationFieldError() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.MissingRequiredConfigurationFieldError.cs");
        public static string MissingRequiredConfigurationSectionError() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.MissingRequiredConfigurationSectionError.cs");
        public static string InvalidConfigurationError() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.InvalidConfigurationError.cs");

        public static string DataProtectionServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.DataProtectionServiceCollectionExtensions.cs");
        public static string ApplicationBuilderExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.ApplicationBuilderExtensions.cs");
        public static string ServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.ServiceCollectionExtensions.cs");
        public static string SameSiteCookiesServiceCollectionExtensions() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.SameSiteCookiesServiceCollectionExtensions.cs");
        public static string AuthInfoHelper() => GetResourceText("Rixian.Extensions.AspNetCore.StartupSourceGenerator.source.AuthInfoHelper.cs");
    }
}
