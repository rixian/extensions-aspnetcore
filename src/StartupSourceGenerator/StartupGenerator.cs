using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StartupSourceGenerator
{
    [Generator]
    public class StartupGenerator : ISourceGenerator
    {
        public GeneratorExecutionContext Context { get; set; }
        public Compilation SecondCompilation { get; set; }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                //var aspnetcoreConfigFile = context.AdditionalFiles.SingleOrDefault(at => at.Path.EndsWith("aspnetcore.json"));
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableDataProtection", out var enableDataProtection);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableWebApi", out var enableWebApi);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableRedis", out var enableRedis);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableOAuth2", out var enableOAuth2);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableRazorPages", out var enableRazorPages);
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.EnableOpenIdConnect", out var enableOpenIdConnect);

                var options = new StartupOptions
                {
                    EnableDataProtection = enableDataProtection is object,
                    EnableWebApi = enableWebApi is object,
                    EnableRedis = enableRedis is object,
                    EnableOAuth2 = enableOAuth2 is object,
                    EnableOpenIdConnect = enableOpenIdConnect is object,
                    EnableRazorPages = enableRazorPages is object,
                };

                SecondCompilation = context.Compilation;

                void AddSource(string filename, string text)
                {
                    context.AddSource(filename, SourceText.From(text, Encoding.UTF8));

                    // we're going to create a new compilation that contains the attribute.
                    // TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
                    CSharpParseOptions options = (SecondCompilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
                    SecondCompilation = SecondCompilation
                        .AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(text, Encoding.UTF8), options));
                }

                // add the attribute text
                //AddSource("EnableRedisAttribute", Sources.GetEnableRedisAttribute());
                //AddSource("EnableDataProtectionAttribute", Sources.GetEnableDataProtectionAttribute());
                //AddSource("EnableWebApiAttribute", Sources.GetEnableWebApiAttribute());

                // retreive the populated receiver 
                //if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                //    return;

                //// get the newly bound attribute, and INotifyPropertyChanged
                //INamedTypeSymbol enableDataProtectionAttributeSymbol = SecondCompilation.GetTypeByMetadataName("Microsoft.AspNetCore.EnableDataProtectionAttribute");
                //INamedTypeSymbol enableRedisAttributeSymbol = SecondCompilation.GetTypeByMetadataName("Microsoft.AspNetCore.EnableRedisAttribute");
                //INamedTypeSymbol enableWebApiAttributeSymbol = SecondCompilation.GetTypeByMetadataName("Microsoft.AspNetCore.EnableWebApiAttribute");


                //IEnumerable<SyntaxNode>? allNodes = SecondCompilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
                //IEnumerable<AttributeSyntax> allAttributes = allNodes.Where((d) => d.IsKind(SyntaxKind.Attribute)).OfType<AttributeSyntax>();



                //var content = aspnetcoreConfigFile.GetText(context.CancellationToken);
                //var options = System.Text.Json.JsonSerializer.Deserialize<StartupOptions>(content.ToString());

                AddSource("BaseStartup", StartupClassGenerator.Generate(options));
                AddSource("ServiceCollectionExtensions", Sources.ServiceCollectionExtensions());
                AddSource("ApplicationBuilderExtensions", Sources.ApplicationBuilderExtensions());
                AddSource("MissingRequiredConfigurationFieldError", Sources.MissingRequiredConfigurationFieldError());
                AddSource("MissingRequiredConfigurationSectionError", Sources.MissingRequiredConfigurationSectionError());
                AddSource("InvalidConfigurationError", Sources.InvalidConfigurationError());

                if (options.EnableDataProtection)
                {
                    AddSource("DataProtectionConfig", Sources.DataProtectionConfig());
                    AddSource("KeyRingConfig", Sources.KeyRingConfig());
                    AddSource("DataProtectionServiceCollectionExtensions", Sources.DataProtectionServiceCollectionExtensions());
                }

                if (options.EnableWebApi)
                {
                    AddSource("ApiConfig", Sources.ApiConfig());
                    AddSource("OAuth2Config", Sources.OAuth2Config());
                    AddSource("AuthInfoHelper", Sources.AuthInfoHelper());
                }

                if (options.EnableRazorPages)
                {
                    AddSource("OAuth2Config", Sources.OAuth2Config());
                    AddSource("SlugifyParameterTransformer", Sources.SlugifyParameterTransformer());
                    AddSource("SameSiteCookiesServiceCollectionExtensions", Sources.SameSiteCookiesServiceCollectionExtensions());
                    AddSource("AuthInfoHelper", Sources.AuthInfoHelper());
                }

                if (options.EnableRedis)
                {
                    AddSource("RedisConfig", Sources.RedisConfig());
                }

                if (options.EnableOpenIdConnect)
                {
                    AddSource("OpenIdConnectConfig", Sources.OpenIdConnectConfig());
                }


                //ImmutableArray<AttributeSyntax> attributes = 
                //    .ToImmutableArray();

                //SecondCompilation.Emit(@"C:\temp\SgTest\Test.dll");
            }
            catch (Exception ex)
            {
            }
            //Console.WriteLine("*** HI FROM SOURCE GENERATOR ***");
        }


        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<AttributeSyntax> Attributes { get; } = new List<AttributeSyntax>();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // any field with at least one attribute is a candidate for property generation
                if (syntaxNode is AttributeSyntax attributeSyntax)
                {
                    Attributes.Add(attributeSyntax);
                }
            }
        }
    }
}
