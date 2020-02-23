using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rixian.Extensions.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuildeExtensions
    {

        public static IApplicationBuilder ConfigureApplicationBuilder(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment environment)
        {
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
