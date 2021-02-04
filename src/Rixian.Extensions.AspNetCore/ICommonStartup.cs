﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rixian.Extensions.AspNetCore
{
    public interface ICommonStartup
    {
        IConfiguration Configuration { get; }
        IWebHostEnvironment Environment { get; }

        IServiceCollection? Services { get; }
        IApplicationBuilder? App { get; }
    }
}
