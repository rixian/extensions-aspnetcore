// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class StartupBuilder
    {
        public ICollection<Action<WebHostBuilderContext, IServiceCollection>> ServiceConfigurators { get; } = new List<Action<WebHostBuilderContext, IServiceCollection>>();

        public Action<WebHostBuilderContext, IApplicationBuilder, IEndpointRouteBuilder>? ConfigureEndpoints { get; set; }

        public Action<WebHostBuilderContext, IApplicationBuilder>? PreviewUseRouting { get; set; }

        public Action<WebHostBuilderContext, IApplicationBuilder>? PreviewUseEndpoints { get; set; }

        public Action<WebHostBuilderContext, IApplicationBuilder>? PreviewUseAuthentication { get; set; }

        public Action<WebHostBuilderContext, IApplicationBuilder>? PreviewUseAuthorization { get; set; }

        public virtual void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        { }
    }
}
