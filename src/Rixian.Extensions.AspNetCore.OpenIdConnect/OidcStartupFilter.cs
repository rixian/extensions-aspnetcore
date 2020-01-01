// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore.OpenIdConnect
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Startup Filter for adding AuthN and AuthZ to the host.
    /// </summary>
    public class OidcStartupFilter : IStartupFilter
    {
        /// <inheritdoc/>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseAuthentication();
                app.UseAuthorization();
                next(app);
            };
        }
    }
}
