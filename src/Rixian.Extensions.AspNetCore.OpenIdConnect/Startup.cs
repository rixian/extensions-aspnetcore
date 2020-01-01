// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.OpenIdConnect.Startup))]

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    /// <summary>
    /// Additional OpenID Connect startup services.
    /// </summary>
    public class Startup : IHostingStartup
    {
        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            _ = builder
                .ConfigureServices((context, services) =>
                {
                    ILogger<Startup> logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
                    OpenIdConnectConfig? options = context.Configuration.GetSection("Identity")?.Get<OpenIdConnectConfig>();
                    if (options == null)
                    {
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            // Do nothing.
                            logger.LogWarning("[IDENTITY] No configuration section named 'Identity' found, and running in Development. Identity will not be enabled.");
                        }
                        else
                        {
                            throw new InvalidOperationException("[IDENTITY] No configuration section named 'Identity' found, and running in as non-Development. Identity configuration must be provided for non-Development applications.");
                        }
                    }
                    else
                    {
                        Errors.Result isValid = options.CheckRequiredValues(); // Provides the required null checks.

                        if (isValid.IsSuccess)
                        {
                            this.AddHealthCheckServices(services, options.Authority!, options.AuthorityHealthEndpoint);
                            services.AddTransient<IStartupFilter, OidcStartupFilter>();

                            if (options.Api != null)
                            {
                                this.AddAuthenticationServices(services, options.Api.Name!, options.Api.Secret, options.Authority!, context.HostingEnvironment);
                            }
                        }
                        else if (context.HostingEnvironment.IsDevelopment())
                        {
                            // Do nothing.
                            logger.LogWarning("[IDENTITY] Invalid configuration specified, and running in Development. Identity will not be enabled. {Error}", isValid.Error);
                        }
                        else
                        {
                            throw new ErrorException(isValid.Error, "Identity configuration must be provided for non-Development applications.");
                        }
                    }
                });
        }

        private void AddHealthCheckServices(IServiceCollection services, string authority, string? healthEndpoint = null)
        {
            healthEndpoint ??= "/.well-known/openid-configuration";

            _ = services.AddHealthChecks()
                .AddUrlGroup(new Uri(new Uri(authority), healthEndpoint), "oidc", tags: new[] { "services" });
        }

        private void AddAuthenticationServices(IServiceCollection services, string apiName, string? apiSecret, string authority, IWebHostEnvironment hostEnvironment)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = true;
                    options.ApiName = apiName;
                    options.ApiSecret = apiSecret;
                });

            IdentityModelEventSource.ShowPII = hostEnvironment.IsDevelopment();
        }
    }
}
