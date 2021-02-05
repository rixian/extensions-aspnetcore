using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rixian.Extensions.AspNetCore.Demo.CompleteRazorPage
{
    public class Startup : Rixian.Extensions.AspNetCore.BaseStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            : base(configuration, environment)
        {
            //Microsoft.Extensions.DependencyInjection.ApplicationBuilderExtensions
        }
    }
}
