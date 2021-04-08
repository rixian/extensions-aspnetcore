// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Source generator for creating a configurable Startup class for AspNet Core.
    /// </summary>
    [Generator]
    public partial class StartupSourceGenerators : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableDataProtection", out var enableDataProtection);
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableWebApi", out var enableWebApi);
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableRedis", out var enableRedis);
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableOAuth2", out var enableOAuth2);
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableRazorPages", out var enableRazorPages);
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableOpenIdConnect", out var enableOpenIdConnect);

            IEnumerable<NamespaceDeclarationSyntax>? namespaces = context.Compilation.SyntaxTrees?
                .FirstOrDefault()?
                .GetRoot()?
                .DescendantNodes()?
                .OfType<NamespaceDeclarationSyntax>()
                .Where(n => n
                    .DescendantNodes()?
                    .OfType<ClassDeclarationSyntax>()?
                    .Any(c => string.Equals(c.Identifier.NormalizeWhitespace().ToFullString(), "Program")) ?? false);
            string namespaceName = namespaces?.SingleOrDefault()?.Name?.NormalizeWhitespace()?.ToFullString() ?? "Rixian.Extensions.AspNetCore";

            var options = new StartupOptions
            {
                Namespace = namespaceName,
                EnableDataProtection = enableDataProtection is object,
                EnableWebApi = enableWebApi is object,
                EnableRedis = enableRedis is object,
                EnableOAuth2 = enableOAuth2 is object,
                EnableOpenIdConnect = enableOpenIdConnect is object,
                EnableRazorPages = enableRazorPages is object,
            };

            void AddSource(string filename, string text)
            {
                context.AddSource(filename, SourceText.From(text, Encoding.UTF8));
            }

            AddSource("Startup.partial", StartupClassBuilder.Generate(options));
            AddSource("MissingRequiredConfigurationFieldError", Sources.MissingRequiredConfigurationFieldError());
            AddSource("MissingRequiredConfigurationSectionError", Sources.MissingRequiredConfigurationSectionError());
            AddSource("InvalidConfigurationError", Sources.InvalidConfigurationError());

            var extraFiles = new Dictionary<string, string>();

            if (options.EnableDataProtection)
            {
                extraFiles["DataProtectionConfig"] = Sources.DataProtectionConfig();
                extraFiles["KeyRingConfig"] = Sources.KeyRingConfig();
                extraFiles["DataProtectionServiceCollectionExtensions"] = Sources.DataProtectionServiceCollectionExtensions();
            }

            if (options.EnableWebApi)
            {
                extraFiles["ApiConfig"] = Sources.ApiConfig();
                extraFiles["OAuth2Config"] = Sources.OAuth2Config();
                extraFiles["AuthInfoHelper"] = Sources.AuthInfoHelper();
            }

            if (options.EnableRazorPages)
            {
                extraFiles["OAuth2Config"] = Sources.OAuth2Config();
                extraFiles["SlugifyParameterTransformer"] = Sources.SlugifyParameterTransformer();
                extraFiles["SameSiteCookiesServiceCollectionExtensions"] = Sources.SameSiteCookiesServiceCollectionExtensions();
                extraFiles["AuthInfoHelper"] = Sources.AuthInfoHelper();
            }

            if (options.EnableRedis)
            {
                extraFiles["RedisConfig"] = Sources.RedisConfig();
            }

            if (options.EnableOpenIdConnect)
            {
                extraFiles["OpenIdConnectConfig"] = Sources.OpenIdConnectConfig();
            }

            foreach (KeyValuePair<string, string> extraFile in extraFiles)
            {
                AddSource(extraFile.Key, extraFile.Value);
            }
        }

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
