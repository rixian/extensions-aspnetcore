//// Copyright (c) Rixian. All rights reserved.
//// Licensed under the Apache License, Version 2.0 license.

//namespace Rixian.Extensions.AspNetCore
//{
//    using Microsoft.AspNetCore.Builder;
//    using Microsoft.AspNetCore.Hosting;
//    using Microsoft.AspNetCore.HttpOverrides;
//    using Microsoft.AspNetCore.Mvc;
//    using Microsoft.AspNetCore.Mvc.Versioning;
//    using Microsoft.Extensions.Configuration;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.Hosting;
//    using Microsoft.Extensions.Logging;
//    using Microsoft.IdentityModel.Logging;
//    using Rixian.Extensions.Errors;
//    using System;
//    using System.Linq;
//    using System.Text.Json;
//    using System.Text.Json.Serialization;

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
//            services.Configure<ForwardedHeadersOptions>(options =>
//            {
//                options.ForwardedHeaders = ForwardedHeaders.All;

//                // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0#forward-the-scheme-for-linux-and-non-iis-reverse-proxies
//                // Only loopback proxies are allowed by default.
//                // Clear that restriction because forwarders are enabled by explicit configuration.
//                options.KnownNetworks.Clear();
//                options.KnownProxies.Clear();
//            });


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

//            services.AddControllers();
//            services
//                // API EXPLORER
//                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
//                // note: the specified format code will format the version as "'v'major[.minor][-status]"
//                .AddVersionedApiExplorer(
//                    o =>
//                    {
//                        {
//                            o.GroupNameFormat = "'v'VVV";

//                            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
//                            // can also be used to control the format of the API version in route templates
//                            // o.SubstituteApiVersionInUrl = true;
//                        }
//                    });




//            services.AddHealthChecks();
//            services
//                // MVC Basics
//                .AddControllers()
//                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
//                .AddJsonOptions(o =>
//                {
//                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//                });

//            // CORS
//            services.AddCors(o =>
//            {
//                o.AddPolicy("AllowAllOrigins", builder =>
//                {
//                    builder
//                        .AllowAnyOrigin()
//                        .AllowAnyHeader()
//                        .AllowAnyMethod();
//                });
//            });

//            // AUTH
//            services.AddAuthorization();


//            services
//                // API VERSIONING
//                .AddApiVersioning(o =>
//                {
//                    o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new MediaTypeApiVersionReader());
//                    o.AssumeDefaultVersionWhenUnspecified = true;

//                    if (defaultVersion != null)
//                    {
//                        o.DefaultApiVersion = new ApiVersion(defaultVersion.Value);
//                    }

//                    o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
//                    o.ReportApiVersions = true;
//                });









//            // OAuth2
//            OAuth2Config oauth2Options = this.Options.OAuth2;
//            if (oauth2Options == null)
//            {
//                if (this.Environment.IsDevelopment())
//                {
//                    // Do nothing.
//                }
//                else
//                {
//                    throw new InvalidOperationException("[IDENTITY] No configuration section named 'Identity' found, and running in as non-Development. Identity configuration must be provided for non-Development applications.");
//                }
//            }
//            else
//            {
//                Extensions.Errors.Result isValid = oauth2Options.CheckRequiredValues(); // Provides the required null checks.

//                if (isValid.IsSuccess)
//                {
//                    services.AddAuthentication("token")

//                        // JWT Bearer Tokens
//                        .AddJwtBearer("token", o =>
//                        {
//                            {
//                                o.Authority = oauth2Options.Authority!;
//                                o.RequireHttpsMetadata = !this.Environment.IsDevelopment();
//                                o.Audience = oauth2Options!.Name!;

//                                o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

//                                // Forward to the introspection handler (for reference tokens) when the token does not contain a dot.
//                                o.ForwardDefaultSelector = context =>
//                                {
//                                    {
//                                        var header = context.Request.Headers["Authorization"].FirstOrDefault();
//                                        var split = header.Split(' ');

//                                        if (split.Length == 2)
//                                        {
//                                            {
//                                                var scheme = split[0];
//                                                var cred = split[1];

//                                                if (string.Equals(scheme, "bearer", StringComparison.OrdinalIgnoreCase) && cred.Contains('.') == false)
//                                                {
//                                                    {
//                                                        return "introspection";
//                                                    }
//                                                }
//                                            }
//                                        }

//                                        return null;
//                                    }
//                                };
//                            }
//                        })

//                        // Reference Tokens
//                        .AddOAuth2Introspection("introspection", o =>
//                        {
//                            {
//                                o.Authority = oauth2Options.Authority;

//                                o.ClientId = oauth2Options.Name;
//                                o.ClientSecret = oauth2Options.Secret;
//                            }
//                        });

//                    IdentityModelEventSource.ShowPII = this.Environment.IsDevelopment();
//                }
//                else if (this.Environment.IsDevelopment())
//                {
//                    // Do nothing.
//                }
//                else
//                {
//                    throw new ErrorException(isValid.Error, "Identity configuration must be provided for non-Development applications.");
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
//            app.UseCors("AllowAllOrigins");
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

