// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.Api
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Startup Filter for adding CORS to the host.
    /// </summary>
    public class DefaultStartupFilter : IStartupFilter
    {
        private readonly IWebHostEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultStartupFilter"/> class.
        /// </summary>
        /// <param name="environment">The web host environment.</param>
        public DefaultStartupFilter(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                if (this.environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                    app.UseHttpsRedirection();
                }

                app.UseRouting();
                app.UseCors("AllowAllOrigins");
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                next(app);
            };
        }
    }
}
