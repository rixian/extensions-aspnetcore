// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Rixian.Extensions.AspNetCore;

    /// <summary>
    /// Helper extensions for IApplicationBuilder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the IApplication builder with basic values.
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The web host environment.</param>
        /// <returns>The updated IApplicationBuilder.</returns>
        public static IApplicationBuilder ConfigureApplicationBuilder(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (app is null)
            {
                throw new System.ArgumentNullException(nameof(app));
            }

            IEnumerable<StartupBuilder> startupBuilders = app.ApplicationServices.GetRequiredService<IEnumerable<StartupBuilder>>();

            if (environment.IsDevelopment())
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
                builder.PreviewUseRouting?.Invoke(configuration, environment, app);
            }

            app.UseRouting();

            foreach (StartupBuilder startupBuilder in startupBuilders)
            {
                startupBuilder.PreviewUseAuthentication?.Invoke(configuration, environment, app);
            }

            app.UseAuthentication();

            foreach (StartupBuilder startupBuilder in startupBuilders)
            {
                startupBuilder.PreviewUseAuthorization?.Invoke(configuration, environment, app);
            }

            app.UseAuthorization();

            foreach (StartupBuilder startupBuilder in startupBuilders)
            {
                startupBuilder.PreviewUseEndpoints?.Invoke(configuration, environment, app);
            }

            app.UseEndpoints(endpoints =>
            {
                foreach (StartupBuilder startupBuilder in startupBuilders)
                {
                    startupBuilder.ConfigureEndpoints?.Invoke(configuration, environment, app, endpoints);
                }

                endpoints.MapControllers();
            });

            return app;
        }
    }
}
