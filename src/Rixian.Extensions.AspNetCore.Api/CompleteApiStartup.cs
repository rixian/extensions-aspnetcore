//// Copyright (c) Rixian. All rights reserved.
//// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

//namespace Rixian.Extensions.AspNetCore.Api
//{
//    using System;
//    using Microsoft.AspNetCore.Builder;
//    using Microsoft.AspNetCore.Hosting;
//    using Microsoft.Extensions.Configuration;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.Hosting;
//    using Microsoft.Extensions.Logging;
//    using Rixian.Extensions.AspNetCore.DataProtection;
//    using Rixian.Extensions.AspNetCore.OpenIdConnect;
//    using Rixian.Extensions.AspNetCore.StackExchangeRedis;
//    using Rixian.Extensions.Errors;


//    public class CompleteApiStartup
//    {
//        public IConfiguration Configuration { get; }
//        public IWebHostEnvironment Environment { get; }

//        public CompleteApiStartup(IConfiguration configuration, IWebHostEnvironment environment)
//        {
//            Configuration = configuration;
//            Environment = environment;
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.ConfigureForwardedHeadersOptions();

//            ApiConfig? apiOptions = Configuration.GetSection("Api")?.Get<ApiConfig>();
//            DateTime? defaultVersion = null;

//            if (apiOptions == null)
//            {
//                // Do nothing.
//                //logger.LogWarning(Properties.Resources.ConfigurationNotFoundMessage);
//            }

//            apiOptions?.EnsureRequiredValues();
//            if (DateTime.TryParse(apiOptions?.DefaultVersion, out DateTime version))
//            {
//                defaultVersion = version;
//                //logger.LogInformation(Properties.Resources.FoundDefaultVersionMessage, defaultVersion?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
//            }

//            services.AddControllers();

//            services
//                    .AddApiExplorerServices()
//                    .AddMvcServices()
//                    .AddCorsServices()
//                    .AddAuthorizationServices()
//                    .AddApiVersioningServices(defaultVersion);


//            // Data Protection
//            this.AddFullDataProtection();
//            // ===============


//            // OIDC
//            this.AddOpenIdConnect();
//            // ===============


//            // Redis
//            this.AddRedis();
//            // ===============


//            //services.AddSwaggerGen(c =>
//            //{
//            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rixian.Templates.WebApi", Version = "v1" });
//            //});
//        }

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
