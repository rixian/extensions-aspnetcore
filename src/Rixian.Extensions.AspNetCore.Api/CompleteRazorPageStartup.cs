// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Api
{
    using System;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;
    using Rixian.Extensions.AspNetCore.DataProtection;
    using Rixian.Extensions.AspNetCore.OpenIdConnect;
    using Rixian.Extensions.AspNetCore.StackExchangeRedis;
    using Rixian.Extensions.Errors;

    public class CompleteRazorPageStartup : StartupBase, ICommonStartup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public IServiceCollection Services { get; private set; }
        public IApplicationBuilder App { get; private set; }

        public CompleteRazorPageStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            this.Services = services;

            services.ConfigureForwardedHeadersOptions();

            services.Configure<CookiePolicyOptions>(o =>
            {
                o.MinimumSameSitePolicy = SameSiteMode.Strict;
                o.Secure = CookieSecurePolicy.Always;
            });
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddControllersWithViews();
            services.AddMvcServices()
                    .AddCorsServices()
                    .AddAuthorizationServices();
            IMvcBuilder mvcBuilder = services.AddRazorPages(options =>
            {
                options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
            });

            // Data Protection
            this.AddFullDataProtection();

            // OIDC
            IdentityConfig? identityOptions = this.Configuration.GetSection("DataProtection")?.Get<IdentityConfig>();
            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = "oidc";
                })
                .AddCookie(o =>
                {
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.IsEssential = true;
                })
                .AddOpenIdConnect("oidc", o =>
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

                    o.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");

                    o.GetClaimsFromUserInfoEndpoint = true;
                    o.SaveTokens = true;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                    };

                    //if (!this.Environment.IsDevelopment())
                    //{
                    //    o.Events.OnRedirectToIdentityProvider = context =>
                    //    {
                    //        // HACK - Figure out why x-fowarded-proto headers are not coming from Ambassador.
                    //        context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http:", "https:");
                    //        return Task.CompletedTask;
                    //    };
                    //}
                    this.ConfigureOpenIdConnect(o);
                });

            // Redis
            this.AddRedis();

            this.ConfigureServicesCore(services);
        }

        public virtual void ConfigureServicesCore(IServiceCollection services)
        {
        }

        public virtual void ConfigureOpenIdConnect(OpenIdConnectOptions options)
        {
        }

        public override void Configure(IApplicationBuilder app)
        {
            this.App = app;

            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            if (env.IsDevelopment() == false)
            {
                app.UseHttpsScheme();
            }

            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(Constants.CorsAllowAllOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSelfHealthEndpoint();
            app.UseServiceHealthEndpoint();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
