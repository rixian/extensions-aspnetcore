//// Copyright (c) Rixian. All rights reserved.
//// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

//namespace Rixian.Extensions.AspNetCore
//{
//    using Microsoft.AspNetCore.Builder;
//    using Microsoft.AspNetCore.Hosting;
//    using Microsoft.Extensions.Configuration;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.Hosting;
//    using Microsoft.Extensions.Logging;
//    using Rixian.Extensions.Errors;
//    using System;

//    public partial class BaseStartup2
//    {
//        public BaseStartup2(IConfiguration configuration, IWebHostEnvironment environment)
//        {
//            Configuration = configuration;
//            Environment = environment;
//            Options = Configuration.Get<GlobalConfig>();


//            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

//        }

//        public IConfiguration Configuration { get; }
//        public IWebHostEnvironment Environment { get; }
//        public GlobalConfig Options { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.ConfigureForwardedHeadersOptions();

//            Rixian.Extensions.AspNetCore.ApiConfig? apiOptions = this.Options.Api;
//            DateTime? defaultVersion = null;
//            if (apiOptions == null)
//            {
//                // Do nothing.
//            }

//            apiOptions?.EnsureRequiredValues();
//            if (DateTime.TryParse(apiOptions?.DefaultVersion, out DateTime version))
//            {
//                defaultVersion = version;
//            }
//            services.AddHealthChecks();
//            services.AddControllers();

//            services
//                    .AddApiExplorerServices()
//                    .AddMvcServices()
//                    .AddCorsServices()
//                    .AddAuthorizationServices()
//                    .AddApiVersioningServices(defaultVersion);


//            services.AddFullDataProtection(this.Options.DataProtection);

//            // // OIDC
//            // this.AddOpenIdConnect();
//            // // ===============


//            // Redis
//            RedisConfig redisOptions = this.Options.Redis;
//            if (redisOptions == null)
//            {
//                if (this.Environment.IsDevelopment())
//                {
//                    // Do nothing.
//                    //logger.LogWarning(Properties.Resources.ConfigurationMissingMessage);
//                }
//                else
//                {
//                    throw new InvalidOperationException("");// Properties.Resources.RequiredConfigurationMissingMessage);
//                }
//            }
//            else
//            {
//                Extensions.Errors.Result isValid = redisOptions.CheckRequiredValues(); // Provides the required null checks.

//                if (isValid.IsSuccess)
//                {
//                    services.AddStackExchangeRedisCache(o =>
//                    {
//                        o.Configuration = redisOptions.Configuration;
//                        o.InstanceName = redisOptions.InstanceName;
//                    });

//                    services
//                        .AddHealthChecks()
//                        .AddRedis(redisOptions.Configuration);

//                    //logger.LogInformation(Properties.Resources.ConfigurationFoundMessage, redisOptions?.InstanceName);
//                }
//                else if (this.Environment.IsDevelopment())
//                {
//                    //logger.LogWarning(Properties.Resources.InvalidConfigurationMessage, JsonConvert.SerializeObject(isValid.Error, Formatting.Indented));
//                    services.AddDistributedMemoryCache();
//                }
//                else
//                {
//                    throw new ErrorException(isValid.Error, "Redis configuration must be provided for non-Development applications.");
//                }
//            }
//            // ===============
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
//        {
//            if (env.IsDevelopment() == false)
//            {
//                app.UseHttpsScheme();
//            }

//            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0
//            app.UseForwardedHeaders();

//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                //app.UseSwagger();
//                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rixian.Templates.WebApi v1"));
//            }
//            else
//            {
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//                app.UseHttpsRedirection();
//            }

//            app.UseHttpsRedirection();

//            app.UseRouting();
//            app.UseCors(Constants.CorsAllowAllOrigins);
//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseSelfHealthEndpoint();
//            app.UseServiceHealthEndpoint();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}

