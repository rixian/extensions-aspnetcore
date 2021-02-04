using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace StartupSourceGenerator
{
    internal static class Sources
    {
        public static string GetResourceText(string name)
        {
            var assembly = typeof(StartupSourceGenerator.Sources).GetTypeInfo().Assembly;
            using Stream resource = assembly.GetManifestResourceStream(name);
            using StreamReader sr = new StreamReader(resource);
            return sr.ReadToEnd();
        }

        public static string DataProtectionConfig() => GetResourceText("StartupSourceGenerator.source.DataProtectionConfig.cs");
        public static string KeyRingConfig() => GetResourceText("StartupSourceGenerator.source.KeyRingConfig.cs");
        public static string ApiConfig() => GetResourceText("StartupSourceGenerator.source.ApiConfig.cs");
        public static string OAuth2Config() => GetResourceText("StartupSourceGenerator.source.OAuth2Config.cs");
        public static string OpenIdConnectConfig() => GetResourceText("StartupSourceGenerator.source.OpenIdConnectConfig.cs");
        public static string RedisConfig() => GetResourceText("StartupSourceGenerator.source.RedisConfig.cs");
        public static string SlugifyParameterTransformer() => GetResourceText("StartupSourceGenerator.source.SlugifyParameterTransformer.cs");

        public static string MissingRequiredConfigurationFieldError() => GetResourceText("StartupSourceGenerator.source.MissingRequiredConfigurationFieldError.cs");
        public static string MissingRequiredConfigurationSectionError() => GetResourceText("StartupSourceGenerator.source.MissingRequiredConfigurationSectionError.cs");
        public static string InvalidConfigurationError() => GetResourceText("StartupSourceGenerator.source.InvalidConfigurationError.cs");

        public static string DataProtectionServiceCollectionExtensions() => GetResourceText("StartupSourceGenerator.source.DataProtectionServiceCollectionExtensions.cs");
        public static string ApplicationBuilderExtensions() => GetResourceText("StartupSourceGenerator.source.ApplicationBuilderExtensions.cs");
        public static string ServiceCollectionExtensions() => GetResourceText("StartupSourceGenerator.source.ServiceCollectionExtensions.cs");
        public static string SameSiteCookiesServiceCollectionExtensions() => GetResourceText("StartupSourceGenerator.source.SameSiteCookiesServiceCollectionExtensions.cs");
        public static string AuthInfoHelper() => GetResourceText("StartupSourceGenerator.source.AuthInfoHelper.cs");
    }
}
