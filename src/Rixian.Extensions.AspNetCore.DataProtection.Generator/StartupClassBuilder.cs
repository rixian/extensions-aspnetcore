﻿// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Generates the Startup class.
    /// </summary>
    internal class StartupClassBuilder
    {
        /// <summary>
        /// Generates the Startup class.
        /// </summary>
        /// <param name="options">The configuration options for the Startup class.</param>
        /// <returns>The string representation of the Startup class.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace should be followed by blank line", Justification = "Code generator.")]
        public static string Generate(StartupOptions options)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license.

namespace {options.Namespace}
{{
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;
    using Rixian.Extensions.AspNetCore;
    using Rixian.Extensions.Errors;
    using System;
    using System.Linq;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class Startup
    {{
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {{
            Configuration = configuration;
            Environment = environment;
            Options = Configuration.Get<GlobalConfig>();

            {(options.EnableOAuth2 == false ? string.Empty : "System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();")}
        }}

        public IConfiguration Configuration {{ get; }}
        public IWebHostEnvironment Environment {{ get; }}
        public GlobalConfig Options {{ get; }}

        partial void ConfigureServicesCore(IServiceCollection services);
        partial void ConfigurePreviewRouting(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger);
        partial void ConfigurePreviewAuth(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger);
        partial void ConfigurePreviewEndpoints(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger);
        partial void ConfigureEndpoints(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {{
            services.Configure<ForwardedHeadersOptions>(options =>
            {{
                options.ForwardedHeaders = ForwardedHeaders.All;

                // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0#forward-the-scheme-for-linux-and-non-iis-reverse-proxies
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit configuration.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            }});

            {(options.EnableWebApi == false ? string.Empty : @"
            Rixian.Extensions.AspNetCore.ApiConfig? apiOptions = this.Options.Api;
            DateTime? defaultVersion = null;
            if (apiOptions == null)
            {
                // Do nothing.
            }

            apiOptions?.EnsureRequiredValues();
            if (DateTime.TryParse(apiOptions?.DefaultVersion, out DateTime version))
            {
                defaultVersion = version;
            }

            services.AddControllers();
            services
                // API EXPLORER
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as ""'v'major[.minor][-status]""
                .AddVersionedApiExplorer(
                    o =>
                    {{
                        o.GroupNameFormat = ""'v'VVV"";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        // o.SubstituteApiVersionInUrl = true;
                    }});")}

            {(options.EnableRazorPages == false ? string.Empty : @"
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddControllersWithViews();")}


            services.AddHealthChecks();
            services
                // MVC Basics
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(o =>
                {{
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }});

            // CORS
            services.AddCors(o =>
                {{
                    o.AddPolicy(""AllowAllOrigins"", builder =>
                    {{
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }});
                }});

            // AUTH
            services.AddAuthorization();

            {(options.EnableWebApi == false ? string.Empty : @"
            services
                // API VERSIONING
                .AddApiVersioning(o =>
                {
                    o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new MediaTypeApiVersionReader());
                    o.AssumeDefaultVersionWhenUnspecified = true;

                    if (defaultVersion != null)
                    {
                        o.DefaultApiVersion = new ApiVersion(defaultVersion.Value);
                    }

                    o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
                    o.ReportApiVersions = true;
                });")}

            {(options.EnableRazorPages == false ? string.Empty : @"
            services.AddRazorPages(o =>
            {
                o.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            });")}

            {(options.EnableDataProtection == false ? string.Empty : @"
            services.AddFullDataProtection(this.Options.DataProtection);")}


            {(options.EnableRedis == false ? string.Empty : @"
            // Redis
            RedisConfig redisOptions = this.Options.Redis;
            if (redisOptions == null)
            {
                if (this.Environment.IsDevelopment())
                {
                    // Do nothing.
                }
                else
                {
                    throw new InvalidOperationException(""[REDIS] No configuration section named 'Redis' found, and running in as non-Development. Redis configuration must be provided for non-Development applications."");
                }
            }
            else
            {
                Rixian.Extensions.Errors.Result isValid = redisOptions.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    services.AddStackExchangeRedisCache(o =>
                    {
                        o.Configuration = redisOptions.Configuration;
                        o.InstanceName = redisOptions.InstanceName;
                    });

                    // services
                    //     .AddHealthChecks()
                    //     .AddRedis(redisOptions.Configuration);
                }
                else if (this.Environment.IsDevelopment())
                {
                    services.AddDistributedMemoryCache();
                }
                else
                {
                    throw new ErrorException(isValid.Error, ""Redis configuration must be provided for non - Development applications."");
                }
            }
            // ===============")}

            {(options.EnableOAuth2 == false ? string.Empty : @"
            // OAuth2
            OAuth2Config oauth2Options = this.Options.OAuth2;
            if (oauth2Options == null)
            {
                if (this.Environment.IsDevelopment())
                {
                    // Do nothing.
                }
                else
                {
                    throw new InvalidOperationException(""[IDENTITY] No configuration section named 'Identity' found, and running in as non-Development. Identity configuration must be provided for non-Development applications."");
                }
            }
            else
            {
                Rixian.Extensions.Errors.Result isValid = oauth2Options.CheckRequiredValues(); // Provides the required null checks.

                if (isValid.IsSuccess)
                {
                    services.AddAuthentication(""token"")

                        // JWT Bearer Tokens
                        .AddJwtBearer(""token"", o =>
                        {
                            o.Authority = oauth2Options.Authority!;
                            o.RequireHttpsMetadata = !this.Environment.IsDevelopment();
                            o.Audience = oauth2Options!.Name!;

                            o.TokenValidationParameters.ValidTypes = new[] { ""at+jwt"" };

                            // Forwarding works great when the project only has a Web Api. Need to think about forwarding to cookie scheme, if at all...
                            // Forward to the introspection handler (for reference tokens) when the token does not contain a dot.
                            o.ForwardDefaultSelector = context =>
                            {
                                var header = context.Request.Headers[""Authorization""].FirstOrDefault();

                                if (header == null)
                                    return null;

                                var split = header.Split(' ');

                                if (split.Length == 2)
                                {
                                    var scheme = split[0];
                                    var cred = split[1];

                                    if (string.Equals(scheme, ""bearer"", StringComparison.OrdinalIgnoreCase) && cred.Contains('.') == false)
                                    {
                                        return ""introspection"";
                                    }
                                }

                                return null;
                            };
                        })

                        // Reference Tokens
                        .AddOAuth2Introspection(""introspection"", o =>
                        {
                            o.Authority = oauth2Options.Authority;

                            o.ClientId = oauth2Options.Name;
                            o.ClientSecret = oauth2Options.Secret;
                        });

                    IdentityModelEventSource.ShowPII = this.Environment.IsDevelopment();
                }
                else if (this.Environment.IsDevelopment())
                {
                    // Do nothing.
                }
                else
                {
                    throw new ErrorException(isValid.Error, ""Identity configuration must be provided for non-Development applications."");
                }
            }
            // ===============")}

            {(options.EnableOpenIdConnect == false ? string.Empty : @"
            // OpenID Connect
            OpenIdConnectConfig? identityOptions = this.Options.OpenIdConnect;
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = ""oidc"";
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    //o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.IsEssential = true;
                })
                .AddOpenIdConnect(""oidc"", o =>
                {
                    o.Authority = identityOptions.Authority;
                    o.RequireHttpsMetadata = false;

                    o.ClientSecret = identityOptions.ClientSecret;
                    o.ClientId = identityOptions.ClientId;
                    //Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectScope.
                    //Microsoft.IdentityModel.Protocols.OpenIdConnect.ActiveDirectoryOpenIdConnectEndpoints.
                    o.ResponseType = OpenIdConnectResponseType.Code;
                    o.ResponseMode = OpenIdConnectResponseMode.FormPost;
                    o.UsePkce = true;

                    if (!string.IsNullOrWhiteSpace(identityOptions.Scope))
                    {
                        o.Scope.Clear();
                        foreach (var scope in identityOptions.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        {
                            o.Scope.Add(scope);
                        }
                    }

                    o.ClaimActions.MapAllExcept(""iss"", ""nbf"", ""exp"", ""aud"", ""nonce"", ""iat"", ""c_hash"");

                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.SaveTokens = true;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                    };
                });
            // ===============")}

            ConfigureServicesCore(services);
        }}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {{
            if (env.IsDevelopment() == false)
            {{
                app.Use((context, next) =>
                {{
                    context.Request.Scheme = ""https"";
                    return next();
                }});
            }}

            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {{
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint(""/swagger/v1/swagger.json"", ""Rixian.Templates.WebApi v1""));
            }}
            else
            {{
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }}

            app.UseHttpsRedirection();

            {(options.EnableRazorPages == false ? string.Empty : @"
            app.UseStaticFiles();")}

            ConfigurePreviewRouting(app, env, logger);
            app.UseRouting();

            app.UseCors(""AllowAllOrigins"");

            ConfigurePreviewAuth(app, env, logger);
            app.UseAuthentication();

            {(options.EnableOAuth2 == false && options.EnableOpenIdConnect == false ? string.Empty : @"
            app.UseAuthorization();")}

            app.UseHealthChecks(""/self"", new HealthCheckOptions
            {{
                AllowCachingResponses = true,
                Predicate = r => r.Name.Contains(""self""),
                ResponseWriter = async (context, report) =>
                {{
                    var result = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {{
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e => new {{ key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }}),
                        }});
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }},
            }});
            app.UseHealthChecks(""/ready"", new HealthCheckOptions
            {{
                Predicate = r => r.Tags.Contains(""services""),ResponseWriter = async (context, report) =>
                {{
                    var result = System.Text.Json.JsonSerializer.Serialize(
                        new
                        {{
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e => new {{ key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) }}),
                        }});
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }},
            }});

            ConfigurePreviewEndpoints(app, env, logger);
            app.UseEndpoints(endpoints =>
            {{
                endpoints.MapControllers();

            {(options.EnableRazorPages == false ? string.Empty : @"
                endpoints.MapRazorPages();")}

                ConfigureEndpoints(app, env, logger);
            }});
        }}
    }}

    {GetGlobalConfigClass(options)}
}}
");

            return sb.ToString();
        }

        private static string GetGlobalConfigClass(StartupOptions options)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
    public class GlobalConfig
    {");

            if (options.EnableDataProtection)
            {
                sb.AppendLine("        public Rixian.Extensions.AspNetCore.DataProtectionConfig DataProtection { get; set; }");
            }

            if (options.EnableWebApi)
            {
                sb.AppendLine("        public Rixian.Extensions.AspNetCore.ApiConfig Api { get; set; }");
            }

            if (options.EnableOAuth2)
            {
                sb.AppendLine("        public Rixian.Extensions.AspNetCore.OAuth2Config OAuth2 { get; set; }");
            }

            if (options.EnableRedis)
            {
                sb.AppendLine("        public Rixian.Extensions.AspNetCore.RedisConfig Redis { get; set; }");
            }

            if (options.EnableOpenIdConnect)
            {
                sb.AppendLine("        public Rixian.Extensions.AspNetCore.OpenIdConnectConfig OpenIdConnect { get; set; }");
            }

            sb.AppendLine(@"    }");

            return sb.ToString();
        }
    }
}
