// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureServices((ctx, services) =>
                {
                    OpenIdConnectOptions options = ctx.Configuration.GetSection("Identity").Get<OpenIdConnectOptions>();

                    this.AddHealthCheckServices(services, options.Authority, options.AuthorityHealthEndpoint);
                    services.AddTransient<IStartupFilter, OidcStartupFilter>();

                    if (options.Api != null)
                    {
                        this.AddAuthenticationServices(services, options.Api.Name, options.Api.Secret, options.Authority, ctx.HostingEnvironment);
                    }
                });
        }

        private void AddHealthCheckServices(IServiceCollection services, string authority, string? healthEndpoint = null)
        {
            healthEndpoint = healthEndpoint ?? "/.well-known/openid-configuration";

            _ = services.AddHealthChecks()
                .AddUrlGroup(new Uri(new Uri(authority), healthEndpoint), "oidc", tags: new[] { "services" });
        }

        private void AddAuthenticationServices(IServiceCollection services, string apiName, string apiSecret, string authority, IWebHostEnvironment hostEnvironment)
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
