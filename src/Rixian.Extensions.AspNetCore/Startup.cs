// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

[assembly: Microsoft.AspNetCore.Hosting.HostingStartup(typeof(Rixian.Extensions.AspNetCore.Startup))]

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Additional basic startup services.
    /// </summary>
    public class Startup : IHostingStartup
    {
        public Startup()
        {
        }

        /// <inheritdoc/>
        public void Configure(IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .ConfigureServices((ctx, services) =>
                {
                    IEnumerable<StartupBuilder> startupBuilders = services.BuildServiceProvider().GetServices<StartupBuilder>();

                    services
                        .AddHealthChecks()
                        .AddCheck("self", () => HealthCheckResult.Healthy());

                    services.AddTransient<IStartupFilter, HealthStartupFilter>();

                    foreach (StartupBuilder startupBuilder in startupBuilders)
                    {
                        startupBuilder?.ConfigureServices(ctx, services);
                        if (startupBuilder?.ServiceConfigurators != null)
                        {
                            foreach (Action<WebHostBuilderContext, IServiceCollection> configurator in startupBuilder.ServiceConfigurators)
                            {
                                configurator?.Invoke(ctx, services);
                            }
                        }
                    }
                })
                .Configure((ctx, app) =>
                {
                    IEnumerable<StartupBuilder> startupBuilders = app.ApplicationServices.GetRequiredService<IEnumerable<StartupBuilder>>();

                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }
                    else
                    {
                        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                        app.UseHsts();
                        app.UseHttpsRedirection();
                    }

                    foreach (StartupBuilder builder in startupBuilders)
                    {
                        builder.PreviewUseRouting?.Invoke(ctx, app);
                    }

                    app.UseRouting();

                    foreach (StartupBuilder startupBuilder in startupBuilders)
                    {
                        startupBuilder.PreviewUseAuthentication?.Invoke(ctx, app);
                    }

                    app.UseAuthentication();

                    foreach (StartupBuilder startupBuilder in startupBuilders)
                    {
                        startupBuilder.PreviewUseAuthorization?.Invoke(ctx, app);
                    }

                    app.UseAuthorization();

                    foreach (StartupBuilder startupBuilder in startupBuilders)
                    {
                        startupBuilder.PreviewUseEndpoints?.Invoke(ctx, app);
                    }

                    app.UseEndpoints(endpoints =>
                    {
                        foreach (StartupBuilder startupBuilder in startupBuilders)
                        {
                            startupBuilder.ConfigureEndpoints?.Invoke(ctx, app, endpoints);
                        }

                        endpoints.MapControllers();
                    });
                });
        }
    }
}
